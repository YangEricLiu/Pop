using System.Linq;
using AutoMapper;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Common.Startup
{
    public class AutoMapperConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            Mapper.CreateMap<HierarchyAdministratorModel, HierarchyAdministratorDto>();
            Mapper.CreateMap<LogoModel, LogoDto>();
            Mapper.CreateMap<CustomerModel, CustomerDto>().ConvertUsing(
                s =>
                {
                    return new CustomerDto
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        Name = s.Name,
                        Logo = Mapper.Map<LogoDto>(s.Logo),
                        Address = s.Address,
                        Administrators = s.Administrators == null ? null : s.Administrators.Select(a => Mapper.Map<HierarchyAdministratorDto>(a)).ToArray()
                    };
                });

            Mapper.CreateMap<HierarchyAdministratorDto, HierarchyAdministratorModel>();
            Mapper.CreateMap<LogoDto, LogoModel>();
            Mapper.CreateMap<CustomerDto, CustomerModel>().ConvertUsing(
                s =>
                {
                    return new CustomerModel
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        Name = s.Name,
                        Logo = Mapper.Map<LogoModel>(s.Logo),
                        Address = s.Address,
                        Administrators = s.Administrators == null ? null : s.Administrators.Select(a => Mapper.Map<HierarchyAdministratorModel>(a)).ToArray()
                    };
                });

            Mapper.CreateMap<CustomerListItemDto, CustomerListItemModel>();
            Mapper.CreateMap<UserPreviligedCustomerDto, UserPreviligedCustomerModel>();
            Mapper.CreateMap<UserPreviligedCustomerModel, UserPreviligedCustomerDto>();
            Mapper.CreateMap<UserModel, UserDto>().ForMember(d => d.SpStatus, opt => opt.MapFrom(s => (EntityStatus)s.SpStatus));
            Mapper.CreateMap<UserDto, UserModel>().ForMember(d => d.SpStatus, opt => opt.MapFrom(s => (int)s.SpStatus));
            Mapper.CreateMap<HierarchyDto, HierarchyModel>();
            Mapper.CreateMap<HierarchyModel, HierarchyDto>().ForMember(d => d.TimezoneId, opt => opt.Ignore())
                .ForMember(d => d.PathLevel, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.CalcStatus, opt => opt.Ignore())
                .ForMember(d => d.UpdateTime, opt => opt.Ignore())
                .ForMember(d => d.UpdateUser, opt => opt.Ignore());

            Mapper.CreateMap<UserCustomerDto, UserCustomerModel>();
            Mapper.CreateMap<UserCustomerModel, UserCustomerDto>();
            Mapper.CreateMap<GatewayDto, GatewayModel>();
            Mapper.CreateMap<GatewayModel, GatewayDto>();
            Mapper.CreateMap<BuildingLocationModel, BuildingLocationDto>();
            Mapper.CreateMap<BuildingLocationDto, BuildingLocationModel>();
            Mapper.CreateMap<ParkModel, ParkDto>();
            Mapper.CreateMap<ParkDto, ParkModel>();
            Mapper.CreateMap<DeviceModel, DeviceDto>();
            Mapper.CreateMap<DeviceDto, DeviceModel>();
            Mapper.CreateMap<OrganizationDto, OrganizationModel>();
            Mapper.CreateMap<OrganizationModel, OrganizationDto>();
            Mapper.CreateMap<BuildingDto, BuildingModel>();
            Mapper.CreateMap<BuildingModel, BuildingDto>();
            Mapper.AssertConfigurationIsValid();
        }
    }
}