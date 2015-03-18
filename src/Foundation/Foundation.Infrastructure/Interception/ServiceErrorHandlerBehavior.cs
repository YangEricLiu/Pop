using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SE.DSP.Foundation.Infrastructure.Interception
{
    /// <summary>
    /// The service behavior inserts <see cref="ServiceErrorHandler"/> to the app service.
    /// </summary>
    public class ServiceErrorHandlerBehavior : Attribute, IServiceBehavior
    {
        /// <summary>
        /// Insert <see cref="ServiceErrorHandler"/> to the service.
        /// </summary>
        /// <param name="description">The service description.</param>
        /// <param name="host">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase host)
        {
            foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(new ServiceErrorHandler());
            }
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="host"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription description, ServiceHostBase host, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="host"></param>
        public void Validate(ServiceDescription description, ServiceHostBase host)
        {
        }
    }
}