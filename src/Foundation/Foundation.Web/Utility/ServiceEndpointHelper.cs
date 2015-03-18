
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Web
{
    public static class ServiceEndpointHelper
    {
        static NameValueCollection ServicePathConfigurations =
            (NameValueCollection)ConfigurationManager.GetSection("service.endpointAddress");

        public static string GetEndpointConfigName(string moduleName)
        {
            if (String.IsNullOrWhiteSpace(moduleName)) return string.Empty;
            var defaultProtocol = ConfigHelper.Get(ConfiguratgionKey.DEFAULTAPPSERVICEPROTOCOL);

            return moduleName + "." + defaultProtocol;
        }
        
        public static string GetEndpointAddressPrefix(string moduleName)
        {
            var endpointConfigName = GetEndpointConfigName(moduleName);

            if (String.IsNullOrWhiteSpace(endpointConfigName)) return string.Empty;

            var configKey = string.Empty;

            if (endpointConfigName.ToUpper().EndsWith("TCP"))
            {
                configKey = "WcfTCP";
            }
            if (endpointConfigName.ToUpper().EndsWith("HTTP"))
            {
                configKey = "WcfHTTP";
            }
            if (endpointConfigName.ToUpper().EndsWith("PIPE"))
            {
                configKey = "WcfPIPE";
            }

            if (String.Empty == configKey) return string.Empty;

            var prefix = ConfigHelper.Get(configKey);
            return prefix;
        }

        public static System.ServiceModel.EndpointAddress GetEndpointAddress(string moduleName)
        {
            var prefix = GetEndpointAddressPrefix(moduleName);
            var postfix = ServicePathConfigurations[moduleName];

            return new System.ServiceModel.EndpointAddress(prefix + postfix);
        }
    }
}
