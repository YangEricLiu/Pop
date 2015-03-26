using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// App service api client endpoint behavior for add custom message header.
    /// </summary>
    /// <remarks>This class is for internal use only and is not intended to be used directly from your code.</remarks>
    public class CustomHeaderEndpointBehavior : IEndpointBehavior 
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
        /// Add one <see cref="CustomHeaderClientMessageInspector" /> into the <see cref="ClientRuntime.MessageInspectors" /> collections 
        /// to insert custom header into messages.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //for custom header
            clientRuntime.MessageInspectors.Add(new CustomHeaderClientMessageInspector());

            foreach (ClientOperation operation in clientRuntime.Operations)
            {
                //for faultcontract, so not need mark faltcontract at the operation contract
                operation.FaultContractInfos.Add(new FaultContractInfo(ServiceContextConstant.HEADERNAMESPACE, typeof(REMExceptionDetail)));
            }
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="endpoint">N/A</param>
        /// <param name="endpointDispatcher">N/A</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
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