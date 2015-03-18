using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;


namespace SE.DSP.Foundation.Infrastructure.Interception
{
    /// <summary>
    /// Handle the exception which is thrown by app servcie.
    /// </summary>
    public class ServiceErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Logs the exception asyn.
        /// </summary>
        /// <param name="error">The exception which is thrown by the service.</param>
        /// <returns>true if should not abort the session (if there is one) and instance context if the instance context is not Single; otherwise, false.</returns>
        public bool HandleError(Exception error)
        {
            if (error is REMException) //BL exception
            {
                LogHelper.LogException(error, severity: LoggingSeverity.Warning);
            }
            else
            {
                LogHelper.LogException(error, LoggingSeverity.Fatal);
            }

            return true;
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public void ProvideFault(Exception error, MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            //beacuse HandleError is executed in other thread, so can't get the request id from the soap header
            error.Data["RequestId"] = ServiceContext.RequestId;

            //if not provide fault, the common exception will abort the WCF session
            if (!(error is FaultException<REMExceptionDetail>)) //not BL exception
            {
                fault = System.ServiceModel.Channels.Message.CreateMessage(version, new FaultCode("Fatal Error"), error.Message, null);
            }
        }
    }
}