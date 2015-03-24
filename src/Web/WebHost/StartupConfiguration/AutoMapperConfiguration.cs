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
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}