/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: LogServiceBehavior.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : For logging
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SE.DSP.Foundation.Infrastructure.Interception
{
    /// <summary>
    ///  App/Web service behavior for logging.
    /// </summary>
    public class LogServiceBehavior : IServiceBehavior
    {
        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="serviceDescription">N/A.</param>
        /// <param name="serviceHostBase">N/A.</param>
        /// <param name="endpoints">N/A.</param>
        /// <param name="bindingParameters">N/A.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Add <see cref="LogParameterInterceptor" /> to each <see cref="DispatchOperation" />
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channel in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpoint in channel.Endpoints)
                {
                    foreach (DispatchOperation operation in endpoint.DispatchRuntime.Operations)
                    {
                        operation.ParameterInspectors.Add(new LogParameterInterceptor());
                    }
                }
            }
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="serviceDescription">N/A.</param>
        /// <param name="serviceHostBase">N/A.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
