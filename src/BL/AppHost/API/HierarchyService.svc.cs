using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.DataAccess;
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

        public HierarchyService()
        {
            this.hierarchyRepository = IocHelper.Container.Resolve<IHierarchyRepository>();
            this.unitOfWorkProvider = IocHelper.Container.Resolve<IUnitOfWorkProvider>();
            this.hierarchyAdministratorRepository = IocHelper.Container.Resolve<IHierarchyAdministratorRepository>();
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

                unitOfWork.Commit();

                return organization;
            }
        }

        public void DeleteOrganization(long hierarchyId)
        {
            using (var unitOfWork = this.unitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

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
    }
}