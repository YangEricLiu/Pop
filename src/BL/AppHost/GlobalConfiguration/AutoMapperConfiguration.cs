﻿using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.BL.AppHost.GlobalConfiguration
{
    public class AutoMapperConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            AutoMapper.Mapper.CreateMap<SE.DSP.Foundation.Infrastructure.BE.Entities.UserDto, SE.DSP.Pop.BL.API.DataContract.UserDto>().ForMember(d => d.Title, opt => opt.MapFrom(s => SE.DSP.Foundation.Infrastructure.BE.Entities.UserDto.GetTitle(s.Title)));
            AutoMapper.Mapper.CreateMap<Hierarchy, HierarchyDto>().ForMember(d => d.Children, opt => opt.Ignore());
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}