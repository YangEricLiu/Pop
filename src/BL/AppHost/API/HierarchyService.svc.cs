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
        private readonly IDistributionCabinetRepository distributionCabinetRepository;
        private readonly ISingleLineDiagramRepository singleLineDiagramRepository;

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
                                IDistributionRoomRepository distributionRoomRepository,
                                IDistributionCabinetRepository distributionCabinetRepository,
                                ISingleLineDiagramRepository singleLineDiagramRepository) : base(hierarchyRepository, unitOfWorkProvider)
        {
            this.hierarchyAdministratorRepository = hierarchyAdministratorRepository;
            this.gatewayRepository = gatewayRepository;
            this.buildingLocationRepository = buildingLocationRepository;
            this.logoRepository = logoRepository;
            this.ossRepository = ossRepository;
            this.deviceRepository = deviceRepository;
            this.buildingRepository = buildingRepository;
            this.distributionRoomRepository = distributionRoomRepository;
            this.distributionCabinetRepository = distributionCabinetRepository;
            this.singleLineDiagramRepository = singleLineDiagramRepository;
        }

        public OrganizationDto CreateOrganization(OrganizationDto organization)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(organization.Name, organization.Code, organization.ParentId, Entity.Enumeration.HierarchyType.Organization);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                organization.Id = hierarchyEntity.Id;

                organization.Administrators = this.AddHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, organization.Administrators);

                organization.Gateways = this.AddGateways(unitOfWork, hierarchyEntity.Id, organization.Gateways);
  
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

                organization.Administrators = this.UpdateHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, organization.Administrators);

                organization.Gateways = this.UpdateGateways(unitOfWork, hierarchyEntity.Id, organization.Gateways);

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
          
            var result = Mapper.Map<ParkDto>(park);
 
            result.ParentId = hierarchy.ParentId.Value;
            result.Code = hierarchy.Code;
            result.Name = hierarchy.Name;
            result.Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray();
            result.Gateways = gateways.Select(gw => Mapper.Map<GatewayDto>(gw)).ToArray();
            result.Location = Mapper.Map<BuildingLocationDto>(location);
            result.Logo = this.GetLogoByHierarchyId(hierarchyId);

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

                park.Administrators = this.AddHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, park.Administrators);

                park.Gateways = this.AddGateways(unitOfWork, hierarchyEntity.Id, park.Gateways);

                park.Logo = this.AddLogo(unitOfWork, hierarchyEntity.Id, park.Logo);

                var location = Mapper.Map<BuildingLocation>(park.Location);

                this.buildingLocationRepository.Add(unitOfWork, location);

                this.parkRepository.Add(unitOfWork, Mapper.Map<Park>(park));

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

                park.Administrators = this.UpdateHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, park.Administrators);

                park.Gateways = this.UpdateGateways(unitOfWork, hierarchyEntity.Id, park.Gateways);

                park.Logo = this.UpdateLogo(unitOfWork, hierarchyEntity.Id, park.Logo);

                var location = Mapper.Map<BuildingLocation>(park.Location);

                this.buildingLocationRepository.Update(unitOfWork, location);

                this.parkRepository.Update(unitOfWork, Mapper.Map<Park>(park));

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
 
            var result = AutoMapper.Mapper.Map<Device, DeviceDto>(device);

            result.Logo = this.GetLogoByHierarchyId(hierarchyId);
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

                device.Logo = this.AddLogo(unitOfWork, hierarchyEntity.Id, device.Logo);

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

                device.Logo = this.UpdateLogo(unitOfWork, hierarchyEntity.Id, device.Logo);

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
            var building = this.buildingRepository.GetById(hierarchyId);
         
            var result = Mapper.Map<BuildingDto>(building);

            result.Name = hierarchy.Name;
            result.Code = hierarchy.Code;
            result.ParentId = hierarchy.ParentId.Value;  
            result.Administrators = administrators.Select(ad => Mapper.Map<HierarchyAdministratorDto>(ad)).ToArray();
            result.Location = Mapper.Map<BuildingLocationDto>(location);
            result.Logo = this.GetLogoByHierarchyId(hierarchyId);
            result.SingleLineDiagrams = this.GetSingleLineDiagramsByHierarchyId(hierarchyId);

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

                building.Administrators = this.AddHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, building.Administrators);

                building.Logo = this.AddLogo(unitOfWork, hierarchyEntity.Id, building.Logo);

                building.SingleLineDiagrams = this.AddSingleLineDiagrams(unitOfWork, hierarchyEntity.Id, building.SingleLineDiagrams);

                var location = Mapper.Map<BuildingLocation>(building.Location);

                this.buildingLocationRepository.Add(unitOfWork, location);

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

                building.Administrators = this.UpdateHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, building.Administrators);
                building.Logo = this.UpdateLogo(unitOfWork, hierarchyEntity.Id, building.Logo);
                building.SingleLineDiagrams = this.UpdateSingleLineDiagrams(unitOfWork, hierarchyEntity.Id, building.SingleLineDiagrams);

                var location = Mapper.Map<BuildingLocation>(building.Location);

                this.buildingLocationRepository.Update(unitOfWork, location);
 
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
                this.singleLineDiagramRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);

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
            result.SingleLineDiagrams = this.GetSingleLineDiagramsByHierarchyId(hierarchyId);

            return result;
        }

        public DistributionRoomDto CreateDistributionRoom(DistributionRoomDto distributionRoom)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(distributionRoom.Name, distributionRoom.Code, distributionRoom.ParentId, Entity.Enumeration.HierarchyType.DistributionRoom);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                distributionRoom.Id = hierarchyEntity.Id;

                distributionRoom.Administrators = this.AddHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, distributionRoom.Administrators);

                distributionRoom.Gateways = this.AddGateways(unitOfWork, hierarchyEntity.Id, distributionRoom.Gateways);

                distributionRoom.SingleLineDiagrams = this.AddSingleLineDiagrams(unitOfWork, hierarchyEntity.Id, distributionRoom.SingleLineDiagrams);

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

                distributionRoom.Administrators = this.UpdateHierarchyAdministrators(unitOfWork, hierarchyEntity.Id, distributionRoom.Administrators);

                distributionRoom.Gateways = this.UpdateGateways(unitOfWork, hierarchyEntity.Id, distributionRoom.Gateways);

                distributionRoom.SingleLineDiagrams = this.UpdateSingleLineDiagrams(unitOfWork, hierarchyEntity.Id, distributionRoom.SingleLineDiagrams);

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
                this.singleLineDiagramRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        public DistributionCabinetDto GetDistributionCabinetById(long hierarchyId)
        {
            var hierarchy = this.HierarchyRepository.GetById(hierarchyId);
            var distributionCabinet = this.distributionCabinetRepository.GetById(hierarchyId); 
     
            var result = Mapper.Map<DistributionCabinetDto>(distributionCabinet);

            result.ParentId = hierarchy.ParentId.Value;
            result.Code = hierarchy.Code;
            result.Name = hierarchy.Name;
            result.Logo = this.GetLogoByHierarchyId(hierarchyId);
            result.SingleLineDiagrams = this.GetSingleLineDiagramsByHierarchyId(hierarchyId);

            return result;
        }

        public DistributionCabinetDto CreateDistributionCabinet(DistributionCabinetDto distributionCabinet)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = new Hierarchy(distributionCabinet.Name, distributionCabinet.Code, distributionCabinet.ParentId, Entity.Enumeration.HierarchyType.DistributionCabinet);

                hierarchyEntity = this.CreateHierarchy(unitOfWork, hierarchyEntity);

                distributionCabinet.Id = hierarchyEntity.Id;

                distributionCabinet.Logo = this.AddLogo(unitOfWork, hierarchyEntity.Id, distributionCabinet.Logo);

                distributionCabinet.SingleLineDiagrams = this.AddSingleLineDiagrams(unitOfWork, hierarchyEntity.Id, distributionCabinet.SingleLineDiagrams);

                this.distributionCabinetRepository.Add(unitOfWork, Mapper.Map<DistributionCabinet>(distributionCabinet));

                unitOfWork.Commit();

                return distributionCabinet;
            }
        }

        public DistributionCabinetDto UpdateDistributionCabinet(DistributionCabinetDto distributionCabinet)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            {
                var hierarchyEntity = this.HierarchyRepository.GetById(distributionCabinet.Id.Value);

                hierarchyEntity.Name = distributionCabinet.Name;
                hierarchyEntity.Code = distributionCabinet.Code;
                hierarchyEntity.ParentId = distributionCabinet.ParentId;

                this.UpdateHierarchy(unitOfWork, hierarchyEntity);

                distributionCabinet.Logo = this.UpdateLogo(unitOfWork, hierarchyEntity.Id, distributionCabinet.Logo);

                distributionCabinet.SingleLineDiagrams = this.UpdateSingleLineDiagrams(unitOfWork, hierarchyEntity.Id, distributionCabinet.SingleLineDiagrams);

                this.distributionCabinetRepository.Update(unitOfWork, Mapper.Map<DistributionCabinet>(distributionCabinet));

                unitOfWork.Commit();

                return distributionCabinet;
            }
        }

        public void DeleteDistributionCabinet(long hierarchyId)
        {
            using (var unitOfWork = this.UnitOfWorkProvider.GetUnitOfWork())
            { 
                this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);
                this.distributionCabinetRepository.Delete(unitOfWork, hierarchyId);
                this.singleLineDiagramRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);

                this.DeleteHierarchy(unitOfWork, hierarchyId, true);

                unitOfWork.Commit();
            }
        }

        private SingleLineDiagramDto[] GetSingleLineDiagramsByHierarchyId(long hierarchyId)
        {
            var entities = this.singleLineDiagramRepository.GetByHierarchyId(hierarchyId);

            var dtos = entities.Select(sld => Mapper.Map<SingleLineDiagramDto>(sld)).ToArray();

            foreach (var item in dtos)
            {
                item.Content = this.ossRepository.GetById(string.Format(PopClientService.SingleLineDiagramOss, item.Id)).Content;
            }

            return dtos;
        }

        private SingleLineDiagramDto[] AddSingleLineDiagrams(IUnitOfWork unitOfWork, long hierarchyId, SingleLineDiagramDto[] dto)
        { 
            foreach (var item in dto)
            {
                item.HierarchyId = hierarchyId;

                var entity = Mapper.Map<SingleLineDiagram>(item);

                entity = this.singleLineDiagramRepository.Add(unitOfWork, entity);

                item.Id = entity.Id;

                this.ossRepository.Add(unitOfWork, new OssObject(string.Format(PopClientService.SingleLineDiagramOss, entity.Id), item.Content));
            }

            return dto;
        }

        private SingleLineDiagramDto[] UpdateSingleLineDiagrams(IUnitOfWork unitOfWork, long hierarchyId, SingleLineDiagramDto[] dto)
        {
            this.singleLineDiagramRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);
            return this.AddSingleLineDiagrams(unitOfWork, hierarchyId, dto);
        }

        private LogoDto GetLogoByHierarchyId(long hierarchyId)
        {
            var logos = this.logoRepository.GetLogosByHierarchyIds(new long[] { hierarchyId });

            LogoDto logoDto = null;

            if (logos.Length == 1)
            {
                var ossobject = this.ossRepository.GetById(string.Format("img-pic-{0}", logos[0].Id));

                logoDto = new LogoDto
                {
                    Logo = ossobject.Content,
                    Id = logos[0].Id,
                    HierarchyId = hierarchyId
                };
            }

            return logoDto;
        }

        private LogoDto AddLogo(IUnitOfWork unitOfWork, long hierarchyId, LogoDto dto)
        {
            if (dto != null)
            {
                dto.HierarchyId = hierarchyId;

                var logoEntity = this.logoRepository.Add(unitOfWork, Mapper.Map<Logo>(dto));

                this.ossRepository.Add(new OssObject(string.Format("img-pic-{0}", logoEntity.Id), dto.Logo));

                dto.Id = logoEntity.Id;

                return dto;
            }

            return null;
        }

        private LogoDto UpdateLogo(IUnitOfWork unitOfWork, long hierarchyId, LogoDto dto)
        {
            this.logoRepository.DeleteByHierarchyId(unitOfWork, hierarchyId);
            return this.AddLogo(unitOfWork, hierarchyId, dto);
        }

        private HierarchyAdministratorDto[] AddHierarchyAdministrators(IUnitOfWork unitOfWork, long hierarchyId, HierarchyAdministratorDto[] dto)
        {
            var hirarchyAdminEntities = dto.Select(ad => new HierarchyAdministrator(hierarchyId, ad.Name, ad.Title, ad.Telephone, ad.Email)).ToArray();

            return this.hierarchyAdministratorRepository.AddMany(unitOfWork, hirarchyAdminEntities).Select(ha => AutoMapper.Mapper.Map<BL.API.DataContract.HierarchyAdministratorDto>(ha)).ToArray();
        }

        private HierarchyAdministratorDto[] UpdateHierarchyAdministrators(IUnitOfWork unitOfWork, long hierarchyId, HierarchyAdministratorDto[] dto)
        {
            this.hierarchyAdministratorRepository.DeleteAdministratorByHierarchyId(unitOfWork, hierarchyId);
            return this.AddHierarchyAdministrators(unitOfWork, hierarchyId, dto);
        }

        private GatewayDto[] AddGateways(IUnitOfWork unitOfWork, long hierarchyId, GatewayDto[] dto)
        {
            foreach (var gateway in dto)
            {
                gateway.HierarchyId = hierarchyId;
            }

            var gatewayentities = dto.Select(gw => Mapper.Map<Gateway>(gw)).ToArray();

            this.gatewayRepository.UpdateMany(unitOfWork, gatewayentities);

            return dto;
        }

        private GatewayDto[] UpdateGateways(IUnitOfWork unitOfWork, long hierarchyId, GatewayDto[] dto)
        {
            this.gatewayRepository.DeleteGatewayByHierarchyId(unitOfWork, hierarchyId);
            return this.AddGateways(unitOfWork, hierarchyId, dto);
        }
    }
}