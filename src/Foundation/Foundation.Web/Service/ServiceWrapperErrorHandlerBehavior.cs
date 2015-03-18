using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// Handle the exception which is thrown by the web servcie.
    /// </summary>
    public class ServiceWrapperErrorHandlerBehavior : Attribute, IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(new ServiceWrapperErrorHandler());
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }
    }
}