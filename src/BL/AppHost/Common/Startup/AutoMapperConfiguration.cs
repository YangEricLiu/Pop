using System;
using System.Linq;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Interception;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Entity;

namespace SE.DSP.Pop.BL.AppHost.Common.Startup
{
    public class AutoMapperConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            AutoMapper.Mapper.CreateMap<SE.DSP.Foundation.Infrastructure.BE.Entities.UserDto, SE.DSP.Pop.BL.API.DataContract.UserDto>().ForMember(d => d.Title, opt => opt.MapFrom(s => SE.DSP.Foundation.Infrastructure.BE.Entities.UserDto.GetTitle(s.Title))).ForMember(d => d.Customers, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<SE.DSP.Pop.BL.API.DataContract.UserDto, SE.DSP.Foundation.Infrastructure.BE.Entities.UserDto>().ForMember(d => d.Title, opt => opt.MapFrom(s => (UserTitle)Enum.Parse(typeof(UserTitle), s.Title))).ForSourceMember(d => d.Customers, opt => opt.Ignore()).ForMember(d => d.CustomerIds, opt => opt.MapFrom(s => s.Customers.Select(c => c.CustomerId)));
            AutoMapper.Mapper.CreateMap<Hierarchy, HierarchyDto>().ForMember(d => d.Children, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HierarchyDto, Hierarchy>();
            AutoMapper.Mapper.CreateMap<HierarchyAdministrator, HierarchyAdministratorDto>();
            AutoMapper.Mapper.CreateMap<Gateway, GatewayDto>();
            AutoMapper.Mapper.CreateMap<GatewayDto, Gateway>();
            AutoMapper.Mapper.CreateMap<BuildingLocation, BuildingLocationDto>().ForSourceMember(s => s.UpdateTime, opt => opt.Ignore()).ForSourceMember(s => s.UpdateUser, opt => opt.Ignore()).ForSourceMember(s => s.Version, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<BuildingLocationDto, BuildingLocation>().ConvertUsing(s =>
                {
                    return new BuildingLocation(s.BuildingId, s.Latitude, s.Longitude, s.Province, ServiceContext.CurrentUser.Name);
                });
            AutoMapper.Mapper.CreateMap<LogoDto, Logo>().ConvertUsing(s =>
                {
                    return new Logo(s.HierarchyId.Value, ServiceContext.CurrentUser.Name);
                });
            AutoMapper.Mapper.CreateMap<Logo, LogoDto>().ConvertUsing(s =>
            {
                return new LogoDto
                {
                    Id = s.Id,
                    HierarchyId = s.HierarchyId
                };
            });
            AutoMapper.Mapper.CreateMap<Device, DeviceDto>().ConvertUsing(s =>
                {
                    return new DeviceDto
                    {
                        Description = s.Description,
                        Factory = s.Factory,
                        GatewayId = s.GatewayId
                    };
                });

            AutoMapper.Mapper.CreateMap<DeviceDto, Device>().ConvertUsing(s =>
            {
                return new Device(s.Id.Value, s.GatewayId, s.Factory, s.Description);
            });
                  
            AutoMapper.Mapper.CreateMap<ParkDto, Park>().ConvertUsing(s =>
                {
                    return new Park(s.Id.Value, s.FloorSpace, s.BuildingArea);
                });

            AutoMapper.Mapper.CreateMap<Park, ParkDto>().ConvertUsing(s =>
            {
                return new ParkDto
                {
                    BuildingArea = s.BuildingArea,
                    FloorSpace = s.FloorSpace,
                    Id = s.HierarchyId
                };
            });

            AutoMapper.Mapper.CreateMap<DistributionRoomDto, DistributionRoom>().ConvertUsing(s =>
            {
                return new DistributionRoom(s.Id.Value, s.Location, s.Level, s.TransformerVoltage);
            });

            AutoMapper.Mapper.CreateMap<DistributionRoom, DistributionRoomDto>().ConvertUsing(s =>
            {
                return new DistributionRoomDto
                {
                    Id = s.HierarchyId,
                    Level = s.Level,
                    Location = s.Location,
                    TransformerVoltage = s.TransformerVoltage
                };
            });

            AutoMapper.Mapper.CreateMap<DistributionCabinetDto, DistributionCabinet>().ConvertUsing(s =>
            {
                return new DistributionCabinet(s.Id.Value, s.Type, s.Factory, s.ManufactureTime);
            });

            AutoMapper.Mapper.CreateMap<DistributionCabinet, DistributionCabinetDto>().ConvertUsing(s =>
            {
                return new DistributionCabinetDto
                {
                    Id = s.HierarchyId,
                    Type = s.Type,
                    Factory = s.Factory,
                    ManufactureTime = s.ManufactureTime
                };
            });

            AutoMapper.Mapper.CreateMap<BuildingDto, Building>().ConvertUsing(s =>
            {
                return new Building(s.Id.Value, s.BuildingArea, s.FinishingDate);
            });

            AutoMapper.Mapper.CreateMap<Building, BuildingDto>().ConvertUsing(s =>
            {
                return new BuildingDto
                {
                    FinishingDate = s.FinishingDate,
                    BuildingArea = s.BuildingArea,
                    Id = s.HierarchyId
                };
            });

            AutoMapper.Mapper.CreateMap<SingleLineDiagram, SingleLineDiagramDto>().ForMember(d => d.Content, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<SingleLineDiagramDto, SingleLineDiagram>().ConvertUsing(s =>
                {
                    return new SingleLineDiagram(s.HierarchyId, s.Key, s.Order, s.CreateUser);
                });

            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}