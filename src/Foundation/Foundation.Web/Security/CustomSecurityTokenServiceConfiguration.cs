using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;
using SE.DSP.Foundation.Infrastructure.Utils;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;


namespace SE.DSP.Foundation.Web
{
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private static readonly object syncRoot = new object();
        private const string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";

        /// <summary>
        /// Provides a model for creating a single Configuration object for the application. The first call creates a new CustomSecruityTokenServiceConfiguration and 
        /// places it into the current HttpApplicationState using the key "CustomSecurityTokenServiceConfigurationKey". Subsequent calls will return the same
        /// Configuration object.  This maintains any state that is set between calls and improves performance.
        /// </summary>
        public static CustomSecurityTokenServiceConfiguration Current
        {
            get
            {
                HttpApplicationState httpAppState = HttpContext.Current.Application;

                CustomSecurityTokenServiceConfiguration customConfiguration =
                    httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as
                    CustomSecurityTokenServiceConfiguration;

                if (customConfiguration == null)
                {
                    lock (syncRoot)
                    {
                        customConfiguration =
                            httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as
                            CustomSecurityTokenServiceConfiguration;

                        if (customConfiguration == null)
                        {
                            customConfiguration = new CustomSecurityTokenServiceConfiguration();
                            httpAppState.Add(CustomSecurityTokenServiceConfigurationKey, customConfiguration);
                        }
                    }
                }

                return customConfiguration;
            }
        }

        /// <summary>
        /// CustomSecurityTokenServiceConfiguration constructor.
        /// </summary>
        public CustomSecurityTokenServiceConfiguration()
            : base(WebConfigurationManager.AppSettings[STSConstant.IssuerName],
                   new X509SigningCredentials(CertificateUtil.GetCertificate(
                       StoreName.My, StoreLocation.LocalMachine,
                       ConfigHelper.Get(STSConstant.CertificateThumbprint))))
        {
            this.SecurityTokenService = typeof(CustomSecurityTokenService);
        }
    }
}
