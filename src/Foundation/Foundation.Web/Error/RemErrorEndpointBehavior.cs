using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SE.DSP.Foundation.Web
{
    public class RemErrorEndpointBehavior : IEndpointBehavior
    {
        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="endpoint">N/A</param>
        /// <param name="bindingParameters">N/A</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="endpoint">N/A</param>
        /// <param name="clientRuntime">N/A</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="endpoint">N/A</param>
        /// <param name="endpointDispatcher">N/A</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (OperationDescription operationDesc in endpoint.Contract.Operations)
            {
                if (!operationDesc.Behaviors.Contains(typeof(RemErrorOperationBehavior)))
                {
                    operationDesc.Behaviors.Add(new RemErrorOperationBehavior());
                }
            }
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="endpoint">N/A</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}