using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Foundation.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace SE.DSP.Pop.Web.WebHost.StartupConfiguration
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            var errorMessage = "{\"Error\":{\"Code\":1,\"Message\":\"\"}}";

            if (exception is REMException)
            {
                var ex = (REMException)exception;
                var remError = new RemError()
                                {
                                    Code = ex.Detail.ErrorCode,
                                    Messages = ex.Detail.ErrorMessages
                                };
                errorMessage = new StringBuilder("{\"error\":").Append(JsonHelper.Serialize2String(remError)).Append("}").ToString();
            }

            context.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(errorMessage)
            };
        }
    }
}