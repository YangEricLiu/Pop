using SE.DSP.Pop.BL.AppHost.GlobalConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.BL.AppHost.App_Code
{
    public class InitializeService
    {
        public static void AppInitialize()
        {
            IEnumerable<IGlobalConfiguration> configurations = new IGlobalConfiguration[]
                {
                    new AutoMapperConfiguration(), new PetaPocoConfiguration()
                    
                };

            foreach (var configuration in configurations)
            {
                configuration.Configure();
            }
        }
    }
}