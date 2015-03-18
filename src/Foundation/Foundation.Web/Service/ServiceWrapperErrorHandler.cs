using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;

namespace SE.DSP.Foundation.Web
{
    public class ServiceWrapperErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            if (error is FaultException<REMExceptionDetail> || error is FaultException<ValidationFault>) //BL/validation exception
            {
                LogHelper.LogException(error, severity: LoggingSeverity.Warning);
            }
            else
            {
                LogHelper.LogException(error, LoggingSeverity.Fatal);
            }

            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //beacuse HandleError is executed in other thread, so can't get the request id from the soap header
            error.Data["RequestId"] = ServiceWrapperContext.RequestId;

            RemError remError;
            string errorText;

            FaultException<REMExceptionDetail> remEx = error as FaultException<REMExceptionDetail>;

            if (remEx == null)
            {
                FaultException<ValidationFault> validationEx = error as FaultException<ValidationFault>;

                if (validationEx == null) //other exception
                {
                    remError = WebErrorCodeFactory.GetFailingError();
                    errorText = new StringBuilder("{\"" + Constant.ERRORPARAMETERNAME + "\":").Append(JsonHelper.Serialize2String(remError)).Append("}").ToString();
                }
                else //validation exception
                {
                    int errorCode;

                    Int32.TryParse(validationEx.Detail.Details[0].Message, out errorCode);

                    remError = new RemError()
                                   {
                                       Code = WebErrorCodeFactory.GetWebErrorCode(errorCode, ErrorType.Input, Module.Common)
                                   };

                    errorText = new StringBuilder("{\"" + Constant.ERRORPARAMETERNAME + "\":").Append(JsonHelper.Serialize2String(remError)).Append("}").ToString();
                }
            }
            else //BL exception
            {
                remError = new RemError()
                                {
                                    Code = remEx.Detail.ErrorCode,
                                    Messages = remEx.Detail.ErrorMessages
                                };

                errorText = new StringBuilder("{\"" + Constant.ERRORPARAMETERNAME + "\":").Append(JsonHelper.Serialize2String(remError)).Append("}").ToString();
            }

            fault = WebOperationContext.Current.CreateTextResponse(errorText, "text/html");
        }
    }
}