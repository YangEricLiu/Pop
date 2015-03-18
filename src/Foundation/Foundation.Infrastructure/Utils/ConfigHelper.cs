using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public static class ConfigHelper
    {
        static NameValueCollection deployConfig =
            (NameValueCollection)ConfigurationManager.GetSection("deployConfiguration");

        public static String Get(string key)
        {
            if (deployConfig == null) return null;
            return deployConfig.Get(key);
        }

        public static String Get(DeploymentConfigKey key)
        {
            if (deployConfig == null) return null;
            return deployConfig.Get(key.ToString());
        }
    }
}
