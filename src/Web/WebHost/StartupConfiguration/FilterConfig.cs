using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SE.DSP.Pop.Web.WebHost.StartupConfiguration
{
    public class FilterConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ExceptionHandlingFilter());
            config.Filters.Add(new ActionFilter());
        }
    }
}