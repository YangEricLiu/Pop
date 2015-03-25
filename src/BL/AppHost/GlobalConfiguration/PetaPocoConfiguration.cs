using PetaPoco;
using SE.DSP.Pop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.BL.AppHost.GlobalConfiguration
{
    public class PetaPocoConfiguration : IGlobalConfiguration
    {
        public void Configure()
        {
            Mappers.Register(typeof(Hierarchy), new Pop.MSSQL.Mapper.PopMapper());
        }
    }
}