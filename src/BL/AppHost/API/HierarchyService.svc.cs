using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SE.DSP.Foundation.DataAccess;
using SE.DSP.Foundation.DataAccess.Entity;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.API.ErrorCode;
using SE.DSP.Pop.BL.AppHost.Common.Ioc;
using SE.DSP.Pop.Contract;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.API
{
    [IocServiceBehavior]
    public class HierarchyService : BaseHierarchyService, IHierarchyService
    { 
        private readonly IHierarchyAdministratorRepository hierarchyAdministratorRepository;
        private readonly IGatewayRepository gatewayRepository;
        private readonly IBuildingLocationRepository buildingLocationRepository;
        private readonly ILogoRepository logoRepository;
        private readonly IOssRepository ossRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly IBuildingRepository buildingRepository;
        private readonly IParkRepository parkRepository;
        private readonly IDistributionRoomRepository distributionRoomRepository;

        public HierarchyService(
                                IHierarchyRepository hierarchyRepository,
                                IUnitOfWorkProvider unitOfWorkProvider,
                                IHierarchyAdministratorRepository hierarchyAdministratorRepository,
                                IGatewayRepository gatewayRepository,
                                IBuildingLocationRepository buildingLocationRepository,
                                ILogoRepository logoRepository,
                                IOssRepository ossRepository,
                                IDeviceRepository deviceRepository,
                                IBuildingRepository buildingRepository,
                                IParkRepository parkRepository,
                                IDistributionRoomRepository distributionRoomRepository) : base(hierarchyRepository, unitOfWorkProvider)
        {
            this.hierarchyAdministratorRepository = hierarchyAdministratorRepository;
            this.gatewayRepository = gatewayRepository;
            this.buildingLocationRepository = buildingLocationRepository;
            this.logoRepository = logoRepository;
            this.ossRepository = ossRepository;
            this.deviceRepository = deviceRepository;
            this.buildingRepository = buildingRepository;
            this.distributionRoomRepository = distributionRoomRepository;
        }

        public OrganizationDto CreateOrganization(OrganizationDto organization)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(organization.Name, organization.Code, organization.ParentId, Entity.Enumeration.HierarchyType.Organization);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                organization.Id = hierarchyEntity.Id;

                foreach (var gateway in organization.Gateways)
                {
                    gateway.HierarchyId = hierarchyEntity.Id;
                }

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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.HierarchyRepository.GetById(organization.Id.Value);

                hierarchyEntity.Name = organization.Name;
                hierarchyEntity.Code = organization.Code;
                hierarchyEntity.ParentId = organization.ParentId;

                this.UpdateHierarchy(unitOfWork, hierarchyEntity);

                foreach (var gateway in organization.Gateways)
                {
                    gateway.HierarchyId = hierarchyEntity.Id;
                }

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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        public OrganizationDto GetOrganizationById(long hierarchyId)
        {
            var hierarchy = this.HierarchyRepository.GetById(hierarchyId);
            var administrators = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);
            var gateways = this.gatewayRepository.GetByHierarchyId(hierarchyId);

            return new OrganizationDto
            {
                Id = hierarchy.Id,
                ParentId = hierarchy.ParentId.Value,
                Code = hierarchy.Code,
                Name = hierarchy.Name,
                Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray(),
                Gateways = gateways.Select(gw => Mapper.Map<GatewayDto>(gw)).ToArray()
            };
        }

        public ParkDto GetParkById(long hierarchyId)
        {
            var hierarchy = this.HierarchyRepository.GetById(hierarchyId);
            var park = this.parkRepository.GetById(hierarchyId);
            var administrators = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);
            var gateways = this.gatewayRepository.GetByHierarchyId(hierarchyId);
            var location = this.buildingLocationRepository.GetById(hierarchyId);
            var logos = this.logoRepository.GetLogosByHierarchyIds(new long[] { hierarchyId });

            LogoDto logoDto = null;

            if (logos.Length == 1)
            {
                var ossobject = this.ossRepository.GetById(string.Format("img-pic-{0}", logos[0].Id));

                logoDto = new LogoDto
                {
                    Logo = ossobject.Content,
                    Id = logos[0].Id,
                    HierarchyId = hierarchy.Id
                };
            }

            var result = Mapper.Map<ParkDto>(park);
 
            result.ParentId = hierarchy.ParentId.Value;
            result.Code = hierarchy.Code;
            result.Name = hierarchy.Name;
            result.Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray();
            result.Gateways = gateways.Select(gw => Mapper.Map<GatewayDto>(gw)).ToArray();
            result.Location = Mapper.Map<BuildingLocationDto>(location);
            result.Logo = logoDto != null ? Mapper.Map<LogoDto>(logos[0]) : null;

            return result;
        }

        public ParkDto CreatePark(ParkDto park)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(park.Name, park.Code, park.ParentId, Entity.Enumeration.HierarchyType.Site);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                park.Id = hierarchyEntity.Id;

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

                this.parkRepository.Add(unitOfWork, Mapper.Map<Park>(park));

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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.HierarchyRepository.GetById(park.Id.Value);

                hierarchyEntity.Name = park.Name;
                hierarchyEntity.Code = park.Code;
                hierarchyEntity.ParentId = park.ParentId;

                this.UpdateHierarchy(unitOfWork, hierarchyEntity);

                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var hirarchyAdminEntities = park.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                park.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var gatewayentities = park.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                var location = Mapper.Map<BuildingLocation>(park.Location);

                this.buildingLocationRepository.Update(unitOfWork, location);

                this.parkRepository.Update(unitOfWork, Mapper.Map<Park>(park));

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
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyId);
                this.buildingLocationRepository.Delete(unitOfWork, hierarchyId);
                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);
                this.parkRepository.Delete(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        public DeviceDto GetDeviceById(long hierarchyId)
        {
            var hierarchy = this.HierarchyRepository.GetById(hierarchyId);
            var device = this.deviceRepository.GetById(hierarchyId);
            var pics = this.logoRepository.GetLogosByHierarchyIds(new long[] { hierarchyId });

            LogoDto logoDto = null;

            if (pics.Length == 1)
            {
                var ossobject = this.ossRepository.GetById(string.Format("img-pic-{0}", pics[0].Id));

                logoDto = new LogoDto
                {
                    Logo = ossobject.Content,
                    Id = pics[0].Id,
                    HierarchyId = hierarchyId
                };
            }

            var result = AutoMapper.Mapper.Map<Device, DeviceDto>(device);

            result.Picture = logoDto;
            result.Name = hierarchy.Name;
            result.Code = hierarchy.Code;
            result.ParentId = hierarchy.ParentId.Value;

            return result;
        }

        public DeviceDto CreateDevice(DeviceDto device)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(device.Name, device.Code, device.ParentId, Entity.Enumeration.HierarchyType.Device);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                device.Id = hierarchyEntity.Id;

                this.deviceRepository.Add(unitOfWork, Mapper.Map<Device>(device));

                if (device.Picture != null)
                {
                    var logoEntity = this.logoRepository.Add(unitOfWork, new Logo(device.Id.Value));

                    device.Picture.Id = logoEntity.Id;
                    device.Picture.HierarchyId = device.Id;

                    this.ossRepository.Add(unitOfWork, new OssObject(string.Format("img-pic-{0}", logoEntity.Id), device.Picture.Logo));
                }

                unitOfWork.Commit();

                return device;
            }
        }

        public DeviceDto UpdateDevice(DeviceDto device)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.HierarchyRepository.GetById(device.Id.Value);

                this.UpdateHierarchy(unitOfWork, hierarchyEntity);

                this.deviceRepository.Update(unitOfWork, Mapper.Map<DeviceDto, Device>(device));

                this.logoRepository.DeleteByHierarchyId(unitOfWork, device.Id.Value);

                if (device.Picture != null)
                {
                    var logoEntity = this.logoRepository.Add(unitOfWork, new Logo(device.Id.Value));

                    device.Picture.Id = logoEntity.Id;
                    device.Picture.HierarchyId = device.Id;

                    this.ossRepository.Add(unitOfWork, new OssObject(string.Format("img-pic-{0}", logoEntity.Id), device.Picture.Logo));
                }

                return device;
            }
        }

        public void DeleteDevice(long hierarchyId)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.deviceRepository.Delete(unitOfWork, hierarchyId);

                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        public BuildingDto GetBuildingById(long hierarchyId)
        {
            var hierarchy = this.HierarchyRepository.GetById(hierarchyId);
            var administrators = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);
            var location = this.buildingLocationRepository.GetById(hierarchyId);
            var logos = this.logoRepository.GetLogosByHierarchyIds(new long[] { hierarchyId });
            var building = this.buildingRepository.GetById(hierarchyId);

            LogoDto logoDto = null;

            if (logos.Length == 1)
            {
                var ossobject = this.ossRepository.GetById(string.Format("img-pic-{0}", logos[0].Id));

                logoDto = new LogoDto
                {
                    Logo = ossobject.Content,
                    Id = logos[0].Id,
                    HierarchyId = hierarchy.Id
                };
            }

            var result = Mapper.Map<BuildingDto>(building);

            result.Name = hierarchy.Name;
            result.Code = hierarchy.Code;
            result.ParentId = hierarchy.ParentId.Value;  
            result.Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray();
            result.Location = Mapper.Map<BuildingLocationDto>(location);
            result.Logo = logoDto != null ? Mapper.Map<LogoDto>(logos[0]) : null;

            return result;
        }

        public BuildingDto CreateBuilding(BuildingDto building)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(building.Name, building.Code, building.IndustryId, building.ParentId, Entity.Enumeration.HierarchyType.Building);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                var buildingEntity = Mapper.Map<Building>(building);

                buildingEntity = this.buildingRepository.Add(unitOfWork, buildingEntity);

                building.Id = hierarchyEntity.Id;

                building.Location.BuildingId = hierarchyEntity.Id;

                var hirarchyAdminEntities = building.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                building.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                var location = Mapper.Map<BuildingLocation>(building.Location);

                this.buildingLocationRepository.Add(unitOfWork, location);

                if (building.Logo != null)
                {
                    building.Logo.HierarchyId = hierarchyEntity.Id;

                    var logoEntity = this.logoRepository.Add(unitOfWork, Mapper.Map<Logo>(building.Logo));

                    this.ossRepository.Add(new OssObject(string.Format("img-pic-{0}", logoEntity.Id), building.Logo.Logo));
                }

                unitOfWork.Commit();

                return building;
            }
        }

        public BuildingDto UpdateBuilding(BuildingDto building)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.HierarchyRepository.GetById(building.Id.Value);

                hierarchyEntity.Name = building.Name;
                hierarchyEntity.Code = building.Code;
                hierarchyEntity.ParentId = building.ParentId;
                hierarchyEntity.IndustryId = building.IndustryId;

                this.UpdateHierarchy(unitOfWork, hierarchyEntity);

                this.buildingRepository.Update(unitOfWork, Mapper.Map<Building>(building));

                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var hirarchyAdminEntities = building.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                building.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                var location = Mapper.Map<BuildingLocation>(building.Location);

                this.buildingLocationRepository.Update(unitOfWork, location);

                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyEntity.Id);

                if (building.Logo != null)
                {
                    var logoEntity = this.logoRepository.Add(unitOfWork, Mapper.Map<Logo>(building.Logo));

                    this.ossRepository.Add(new OssObject(string.Format("img-pic-{0}", logoEntity.Id), building.Logo.Logo));

                    building.Logo.Id = logoEntity.Id;
                }

                unitOfWork.Commit();

                return building;
            }
        }

        public void DeleteBuilding(long hierarchyId)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId); 
                this.buildingLocationRepository.Delete(unitOfWork, hierarchyId);
                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);
                this.buildingRepository.Delete(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        public DistributionRoomDto GetDistributionRoomById(long hierarchyId)
        {
            var hierarchy = this.HierarchyRepository.GetById(hierarchyId);
            var distributionRoom = this.distributionRoomRepository.GetById(hierarchyId);
            var administrators = this.hierarchyAdministratorRepository.GetByHierarchyId(hierarchyId);
            var gateways = this.gatewayRepository.GetByHierarchyId(hierarchyId);

            var result = Mapper.Map<DistributionRoomDto>(distributionRoom);

            result.ParentId = hierarchy.ParentId.Value;
            result.Code = hierarchy.Code;
            result.Name = hierarchy.Name;
            result.Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray();
            result.Gateways = gateways.Select(gw => Mapper.Map<GatewayDto>(gw)).ToArray();

            return result;
        }

        public DistributionRoomDto CreateDistributionRoom(DistributionRoomDto distributionRoom)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(distributionRoom.Name, distributionRoom.Code, distributionRoom.ParentId, Entity.Enumeration.HierarchyType.DistributionRoom);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                distributionRoom.Id = hierarchyEntity.Id;
 
                var hirarchyAdminEntities = distributionRoom.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                distributionRoom.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                foreach (var gw in distributionRoom.Gateways)
                {
                    gw.HierarchyId = hierarchyEntity.Id;
                }

                var gatewayentities = distributionRoom.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                this.distributionRoomRepository.Add(unitOfWork, Mapper.Map<DistributionRoom>(distributionRoom));
              
                unitOfWork.Commit();

                return distributionRoom;
            }
        }

        public DistributionRoomDto UpdateDistributionRoom(DistributionRoomDto distributionRoom)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.HierarchyRepository.GetById(distributionRoom.Id.Value);

                hierarchyEntity.Name = distributionRoom.Name;
                hierarchyEntity.Code = distributionRoom.Code;
                hierarchyEntity.ParentId = distributionRoom.ParentId;

                this.UpdateHierarchy(unitOfWork, hierarchyEntity);

                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var hirarchyAdminEntities = distributionRoom.Administrators.Select(ad => new HierarchyAdministrator(hierarchyEntity.Id, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

                distributionRoom.Administrators = this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();

                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyEntity.Id);

                var gatewayentities = distributionRoom.Gateways.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

                this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

                this.distributionRoomRepository.Update(unitOfWork, Mapper.Map<DistributionRoom>(distributionRoom));
 
                unitOfWork.Commit();

                return distributionRoom;
            }
        }

        public void DeleteDistributionRoom(long hierarchyId)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
                this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyId);
            
                this.distributionRoomRepository.Delete(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }
    }
}