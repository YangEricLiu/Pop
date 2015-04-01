using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http.Filters;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.Web.WebHost.Common.Exceptions;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Common.Filters
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            LogHelper.LogException(exception);
            
            var error = new ErrorModel()
            {
                Error = "-1",
                Message = new string[] { "Server error" },
            };

            if (exception is ApiException)
            {
                var ex = exception as ApiException;

                error.Error = ex.ErrorCode.ToString();
                error.Message = new string[] { };
            }

            if (exception is FaultException<REMExceptionDetail>)
            {
                var ex = exception as FaultException<REMExceptionDetail>;
               
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