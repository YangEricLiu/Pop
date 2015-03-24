using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Web.WebHost.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.StartupConfiguration
{
    public class AutoMapperConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            AutoMapper.Mapper.CreateMap<SE.DSP.Pop.BL.API.DataContract.UserDto, SE.DSP.Pop.Web.WebHost.Model.UserModel>().ForMember(d => d.SpStatus, opt => opt.MapFrom(s => (int)s.SpStatus));
            AutoMapper.Mapper.CreateMap<SE.DSP.Pop.BL.API.DataContract.HierarchyDto, SE.DSP.Pop.Web.WebHost.Model.HierarchyModel>();
            AutoMapper.Mapper.CreateMap<HierarchyModel, HierarchyDto>().ForMember(d=>d.TimezoneId,opt=>opt.Ignore())
                .ForMember(d=>d.PathLevel,opt=>opt.Ignore())
                .ForMember(d=>d.Status,opt=>opt.Ignore())
                .ForMember(d=>d.CalcStatus,opt=>opt.Ignore())
                .ForMember(d=>d.UpdateTime,opt=>opt.Ignore())
                .ForMember(d=>d.UpdateUser,opt=>opt.Ignore());

            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}