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
                    return new BuildingLocation(s.BuildingId, s.Latitude, s.Longtitude, s.Province, ServiceContext.CurrentUser.Name);
                });
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}