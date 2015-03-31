using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE.DSP.Pop.BL.AppHost.Common;
using SE.DSP.Pop.BL.AppHost.Common.Startup;

namespace SE.DSP.Pop.BL.AppHost.App_Code
{
    public class InitializeService
    {
        public static void AppInitialize()
        {
            IEnumerable<IGlobalConfiguration> configurations = new IGlobalConfiguration[]
                {
                    new AutoMapperConfiguration(), 
                    new PetaPocoConfiguration(),
                    new IocConfiguration()
                };

            foreach (var configuration in configurations)
            {
                configuration.Configure();
            }
        }
    }
}