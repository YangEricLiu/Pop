using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Foundation.Web;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.StartupConfiguration
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            var error = new ErrorModel()
            {
                Error = "0",
                Message = new string[] { "Server error" },
            };

            if (exception is REMException)
            {
                var ex = (REMException)exception;

                error.Error = ex.Detail.ErrorCode;
                error.Message = ex.Detail.ErrorMessages;
            }

            context.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonHelper.Serialize2String(error))
            };
        }
    }
}