using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Foundation.Web.Wcf;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.API.ErrorCode;
using SE.DSP.Pop.Web.WebHost.Common.Exceptions;
using SE.DSP.Pop.Web.WebHost.Model;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    [RoutePrefix("api/gateway")]
    public class GatewayController : ApiController
    {
        private readonly IPopClientService clientService;

        public GatewayController()
        {
            this.clientService = ServiceProxy<IPopClientService>.GetClient("IPopClientService.EndPoint");
        }

        [HttpGet]
        [Route("register")]
        public GatewayRegisterModel Register([FromUri]GatewayRegisterModel model)
        {
            var gateway = new GatewayDto()
            {
                Name = model.BoxName,
                Mac = model.BoxMac
            };

            try
            {
                gateway = this.clientService.RegisterGateway(gateway);

                model.BoxId = gateway.UniqueId;

                return model;
            }
            catch (FaultException<REMExceptionDetail> ex)
            {
                var detailCode = ex.Detail.DetailCode;

                throw new ApiException() { ErrorCode = detailCode };
            }
        }

        [HttpGet]
        [Route("replace")]
        public GatewayRegisterModel Replace([FromUri]GatewayRegisterModel model)
        {
            var gateway = new GatewayDto()
            {
                Name = model.BoxName,
                Mac = model.BoxMac
            };

            try
            {
                gateway = this.clientService.ReplaceGateway(gateway);

                model.BoxId = gateway.UniqueId;

                return model;
            }
            catch (FaultException<REMExceptionDetail> ex)
            {
                var detailCode = ex.Detail.DetailCode;

                throw new ApiException() { ErrorCode = detailCode };
            }
        }
    }
}