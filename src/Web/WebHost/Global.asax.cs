using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using SE.DSP.Pop.Web.WebHost.Common.Startup;

namespace SE.DSP.Pop.Web.WebHost
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void AppInitialize()
        {
            IEnumerable<IGlobalConfiguration> configurations = new IGlobalConfiguration[]
                {
                    new AutoMapperConfiguration(),
                };

            foreach (var configuration in configurations)
            {
                configuration.Configure();
            }
        }

        protected void Application_Start()
        {
            AppInitialize();

            AreaRegistration.RegisterAllAreas();
            
            GlobalConfiguration.Configure(FilterConfig.Register);
        }
    }
}
