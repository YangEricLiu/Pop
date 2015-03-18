/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: 
 * Author	    : Figo
 * Date Created : 2012-01-04
 * Description  : for add output REMError to ajax invoker.
--------------------------------------------------------------------------------------------*/

using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SE.DSP.Foundation.Web
{
    public class RemErrorOperationBehavior : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Add the error out prameter to the operationDescription.
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="dispatchOperation"></param>
        /// <remarks>The value of error is setted by <see cref="AccessControlOperationInvoker" />.</remarks>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            //this has been moved to AccessControlServiceBehavior, because here is too late for modify operationDescription
            //if (!operationDescription.Messages[1].Body.Parts.Contains(new System.Xml.XmlQualifiedName(Constant.ERRORPARAMETERNAME, ServiceContextConstant.HEADERNAMESPACE))) //not add duplicated for multiple endpoints(http/https) for web service
            //{
            //    MessagePartDescription errorParameter = new MessagePartDescription(Constant.ERRORPARAMETERNAME, ServiceContextConstant.HEADERNAMESPACE);
            //    errorParameter.Type = typeof(RemError);
            //    errorParameter.Index = operationDescription.Messages[1].Body.Parts.Count;

            //    operationDescription.Messages[1].Body.Parts.Add(errorParameter);
            //}
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}
