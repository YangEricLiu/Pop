using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.StartupConfiguration
{
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var result = actionExecutedContext.Response.Content as ObjectContent;

            var data = new ErrorModel()
            {
                Error = "0",
                Message = new string[] { string.Empty },
                Result = result.Value
            };

            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(typeof(ErrorModel), data, result.Formatter)
            };
        }
    }
}