﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.DataAccess.Entity;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class HierarchyService : ServiceBase, IHierarchyService
    {
        private readonly IHierarchyRepository hierarchyRepository;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly IHierarchyAdministratorRepository hierarchyAdministratorRepository;
        private readonly IGatewayRepository gatewayRepository;
        private readonly IBuildingLocationRepository buildingLocationRepository;
        private readonly ILogoRepository logoRepository;
        private readonly IOssRepository ossRepository;

        public HierarchyService()
        {
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
            this.unitOfWorkProvider = IocHelper.Container.Resolve<IUnitOfWorkProvider>();
            this.hierarchyAdministratorRepository = IocHelper.Container.Resolve<IHierarchyAdministratorRepository>();
            this.gatewayRepository = IocHelper.Container.Resolve<IGatewayRepository>();
            this.buildingLocationRepository = IocHelper.Container.Resolve<IBuildingLocationRepository>();
            this.logoRepository = IocHelper.Container.Resolve<ILogoRepository>();
            this.ossRepository = IocHelper.Container.Resolve<IOssRepository>();
        }

        public HierarchyDto GetHierarchyTree(long rootId)
        {
            var user = ServiceContext.CurrentUser;
            var entity = this.hierarchyRepository.GetById(rootId);

            if (entity == null)
            {
                return null;
            }

            var hierarchy = Mapper.Map<HierarchyDto>(entity);

            var children = this.hierarchyRepository.GetByParentId(rootId);
            if (children != null && children.Length > 0)
            {
                List<HierarchyDto> list = new List<HierarchyDto>();
                foreach (var child in children)
                {
                    list.Add(this.GetHierarchyTree(child.Id));
                }

                hierarchy.Children = list.ToArray();
            }

            return hierarchy;
        }

        public HierarchyDto CreateHierarchy(HierarchyDto hierarchy)
        {
            var entity = AutoMapper.Mapper.Map<Hierarchy>(hierarchy);

            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                entity.UpdateTime = DateTime.Now;
                entity.TimezoneId = 1;

                if (hierarchy.Type != HierarchyType.Customer && !this.DoesHierarchyHaveParent(hierarchy))
                {
                    throw new ConcurrentException(Layer.BL, Module.Hierarchy, Convert.ToInt32(999));
                }

                if (this.IsHierarchyCodeDuplicate(hierarchy)) 
                {
                    throw new BusinessLogicException(Layer.BL, Module.Hierarchy, Convert.ToInt32(999));
                }

                if (this.IsHierarchyNameDuplicate(hierarchy)) 
                {
                    throw new BusinessLogicException(Layer.BL, Module.Hierarchy, Convert.ToInt32(999));
                }

                if (hierarchy.Type == HierarchyType.Organization)
                {
                    if (this.IsOrganizationNestingOverLimitation(hierarchy)) 
                    {
                        throw new BusinessLogicException(Layer.BL, Module.Hierarchy, Convert.ToInt32(999));
                    }
                }

                entity = this.hierarchyRepository.Add(entity);

                unitOfWork.Commit();
            }

            var dto = AutoMapper.Mapper.Map<HierarchyDto>(entity);

            return dto;
        }

        public void DeleteHierarchy(long hierarchyId, bool isRecursive)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                this.DeleteHierarchy(unitOfWork, hierarchyId, isRecursive);

                unitOfWork.Commit();
            }
        }

        public void UpdateHierarchy(BL.API.DataContract.HierarchyDto hierarchy)
        {
            var entity = AutoMapper.Mapper.Map<Hierarchy>(hierarchy);

            this.hierarchyRepository.Update(entity);
        }

        public OrganizationDto CreateOrganization(OrganizationDto organization)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(organization.Name);

                hierarchyEntity = this.hierarchyRepository.Add(unitOfWork, hierarchyEntity);

                organization.HierarchyId = hierarchyEntity.Id;

                var hirarchyAdminEntities = organization.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                organization.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                var gatewayentities = organization.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                unitOfWork.Commit();

                return organization;
            }
        }

        public OrganizationDto UpdateOrganization(OrganizationDto organization)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.hierarchyRepository.GetById(organization.HierarchyId.Value);

                hierarchyEntity.Name = organization.Name;

                this.hierarchyRepository.Update(unitOfWork, hierarchyEntity);

                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var hirarchyAdminEntities = organization.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                organization.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var gatewayentities = organization.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                unitOfWork.Commit();

                return organization;
            }
        }

        public void DeleteOrganization(long hierarchyId)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        public OrganizationDto GetOrganizationById(long hierarchyId)
        {
            var hierarchy = this.hierarchyRepository.GetById(hierarchyId);
            var administrators = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);
            var gateways = this.gatewayRepository.GetByHierarchyId(hierarchyId);

            return new OrganizationDto
            {
                HierarchyId = hierarchy.Id,
                Name = hierarchy.Name,
                Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray(),
                Gateways = gateways.Select(gw => Mapper.Map<GatewayDto>(gw)).ToArray()
            };
        }

        public ParkDto GetParkById(long hierarchyId)
        {
            var hierarchy = this.hierarchyRepository.GetById(hierarchyId);
            var administrators = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);
            var gateways = this.gatewayRepository.GetByHierarchyId(hierarchyId);
            var location = this.buildingLocationRepository.GetById(hierarchyId);
            var logos = this.logoRepository.GetLogosByHierarchyIds(new long[] { hierarchyId });

            return new ParkDto
            {
                HierarchyId = hierarchy.Id,
                Name = hierarchy.Name,
                Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray(),
                Gateways = gateways.Select(gw => Mapper.Map<GatewayDto>(gw)).ToArray(),
                Location  = Mapper.Map<BuildingLocationDto>(location),
                Logo = logos.Length > 0 ? Mapper.Map<LogoDto>(logos[0]) : null
            };
        }

        public ParkDto CreatePark(ParkDto park)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(park.Name);

                hierarchyEntity = this.hierarchyRepository.Add(unitOfWork, hierarchyEntity);

                park.HierarchyId = hierarchyEntity.Id;

                park.Location.BuildingId = hierarchyEntity.Id;

                var hirarchyAdminEntities = park.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                park.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                foreach (var gw in park.Gateways)
                {
                    gw.HierarchyId = hierarchyEntity.Id;
                }

                var gatewayentities = park.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                var location = Mapper.Map<BuildingLocation>(park.Location);

                this.buildingLocationRepository.Add(unitOfWork, location);

                if (park.Logo != null)
                {
                    park.Logo.HierarchyId = hierarchyEntity.Id;

                    var logoEntity = this.logoRepository.Add(unitOfWork, Mapper.Map<Logo>(park.Logo));

                    this.ossRepository.Add(new OssObject(string.Format("img-pic-{0}", logoEntity.Id), park.Logo.Logo));
                }

                unitOfWork.Commit();

                return park;
            }
        }

        public ParkDto UpdatePark(ParkDto park)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.hierarchyRepository.GetById(park.HierarchyId.Value);

                hierarchyEntity.Name = park.Name;

                this.hierarchyRepository.Update(unitOfWork, hierarchyEntity);

                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var hirarchyAdminEntities = park.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                park.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var gatewayentities = park.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                var location = Mapper.Map<BuildingLocation>(park.Location);

                this.buildingLocationRepository.Update(unitOfWork, location);

                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyEntity.Id);

                if (park.Logo != null)
                {
                    var logoEntity = this.logoRepository.Add(unitOfWork, Mapper.Map<Logo>(park.Logo));
 
                    this.ossRepository.Add(new OssObject(string.Format("img-pic-{0}", logoEntity.Id), park.Logo.Logo));

                    park.Logo.Id = logoEntity.Id;
                }

                unitOfWork.Commit();

                return park;
            }
        }

        public void DeletePark(long hierarchyId)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyId);
                this.buildingLocationRepository.Delete(unitOfWork, hierarchyId);
                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        #region validation
        private bool IsHierarchyCodeDuplicate(HierarchyDto hierarchy)
        {
            if (hierarchy.Id != 0)
            {
                if (this.hierarchyRepository.RetrieveSiblingHierarchyCountByCodeUnderParentCustomer(hierarchy.Id, hierarchy.Code, hierarchy.CustomerId) > 0)
                {
                    return true;
                }
            }
            else
            {
                if (this.hierarchyRepository.RetrieveChildHierarchyCountByCodeUnderParentCustomer(hierarchy.Code, hierarchy.CustomerId) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsHierarchyNameDuplicate(HierarchyDto hierarchy)
        {
            if (hierarchy.ParentId.HasValue)
            {
                if (hierarchy.Id != 0)
                {
                    if (this.hierarchyRepository.RetrieveSiblingHierarchyCountByNameUnderParentHierarchy(hierarchy.Id, hierarchy.Name, hierarchy.ParentId.Value) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (this.hierarchyRepository.RetrieveChildHierarchyCountByNameUnderParentHierarchy(hierarchy.Name, hierarchy.ParentId.Value) > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                if (hierarchy.Id != 0)
                {
                    if (this.hierarchyRepository.RetrieveSiblingHierarchyCountByNameUnderParentCustomer(hierarchy.Id, hierarchy.Name, hierarchy.CustomerId) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (this.hierarchyRepository.RetrieveChildHierarchyCountByNameUnderParentCustomer(hierarchy.Name, hierarchy.CustomerId) > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool IsOrganizationNestingOverLimitation(HierarchyDto organization)
        {
            if (organization.ParentId.HasValue)
            {
                return this.hierarchyRepository.RetrieveAncestorAndSelfOrganizationCount(organization.ParentId.Value) + 1 > 5;
            }
            else
            {
                return false;
            }
        }

        private bool DoesHierarchyHaveParent(HierarchyDto hierarchy)
        {
            if (hierarchy.ParentId.HasValue)
            {
                Hierarchy parentHierarchy = this.hierarchyRepository.GetById(hierarchy.ParentId.Value);

                if (parentHierarchy == null)
                {
                    return false;
                }
                else
                {
                    hierarchy.CustomerId = parentHierarchy.Type == SE.DSP.Pop.Entity.Enumeration.HierarchyType.Customer ? parentHierarchy.Id : parentHierarchy.CustomerId;
                    return true;
                }
            }
            else
            {
                return this.hierarchyRepository.GetById(hierarchy.CustomerId) != null;
            }
        }
        #endregion

        private void DeleteHierarchy(IUnitOfWork unitOfWork, long hierarchyId, bool isRecursive)
        {
            if (!isRecursive)
            {
                var children = this.hierarchyRepository.GetByParentId(hierarchyId);

                if (children != null && children.Length > 0)
                {
                    throw new BusinessLogicException(Layer.BL, Module.Hierarchy, Convert.ToInt32(999));
                }

                this.hierarchyRepository.Delete(unitOfWork, hierarchyId);
            }
            else
            {
                Func<HierarchyDto, IEnumerable<long>> collector = null;
                collector = (tree) =>
                {
                    List<long> ids = new List<long>();

                    ids.Add(tree.Id);

                    if (tree.Children != null && tree.Children.Length > 0)
                    {
                        foreach (HierarchyDto hierarchy in tree.Children)
                        {
                            ids.AddRange(collector(hierarchy));
                        }
                    }

                    return ids;
                };

                var root = this.GetHierarchyTree(hierarchyId);
                var list = collector(root);

                foreach (var id in list)
                {
                    this.hierarchyRepository.Delete(unitOfWork, id);
                }
            }
        }
    }
}