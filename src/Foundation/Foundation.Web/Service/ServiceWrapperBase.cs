
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// The abstract base class for web service api wrapper
    /// </summary>
    /// <remarks>
    /// <para> 
    /// This class inherits from <see cref="ContextBoundObject" /> and applys <see cref="InvokeContextAttribute" /> 
    /// for tracing the methods invoke chain.
    /// </para> 
    /// <para> 
    /// Applys <see cref="ServiceWrapperExceptionHandlerAttribute" /> for handling the exceptions those are not be catched.
    /// </para>
    /// <para> 
    /// Applys <see cref="ServiceBehaviorAttribute" /> for setting <see cref="ServiceBehavior.InstanceContextMode" />
    /// to <see cref="InstanceContextMode.PerCall" /> and <see cref="ServiceBehavior.ConcurrencyMode" /> to <see cref="ConcurrencyMode.Single" />
    /// </para>
    /// <para> 
    /// Applys <see cref="AspNetCompatibilityRequirements" /> for setting <see cref="AspNetCompatibilityRequirements.RequirementsMode" /> 
    /// to <see cref="AspNetCompatibilityRequirementsMode.Required" />
    /// </para>
    /// </remarks>
    [ServiceWrapperErrorHandlerBehavior]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public abstract class ServiceWrapperBase
    {
        protected NameValueCollection ServicePathConfigurations = (NameValueCollection)ConfigurationManager.GetSection("serviceClientConfiguration");

        public abstract string DefaultEndpointConfigurationName { get; }

        protected virtual string GetEndpointAddressPrefix()
        {
            if (String.IsNullOrWhiteSpace(DefaultEndpointConfigurationName)) return string.Empty;

            var configKey = string.Empty;

            if (DefaultEndpointConfigurationName.ToUpper().EndsWith("TCP"))
            {
                configKey = "WCF.TCP";
            }
            if (DefaultEndpointConfigurationName.ToUpper().EndsWith("HTTP"))
            {
                configKey = "WCF.HTTP";
            }
            if (DefaultEndpointConfigurationName.ToUpper().EndsWith("PIPE"))
            {
                configKey = "WCF.PIPE";
            }

            if (String.Empty == configKey) return string.Empty;

            var prefix = ConfigurationManager.AppSettings[configKey];
            return prefix;
        }
    }
}