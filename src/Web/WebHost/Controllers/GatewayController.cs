using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GatewayController : ApiController
    {
        private readonly IPopClientService clientService;

        public GatewayController()
        {
            this.clientService = ServiceProxy<IPopClientService>.GetClient("IPopClientService.EndPoint");
        }

        [HttpGet]
        public GatewayRegisterResultModel Register(GatewayRegisterModel model)
        {
            var gateway = new GatewayDto()
            {
                Name = model.BoxName,
                Mac = model.BoxMac
            };

            try
            {
                gateway = this.clientService.RegisterGateway(gateway);

                return new GatewayRegisterResultModel()
                {
                    BoxId = gateway.UniqueId,
                    Timestamp = model.Timestamp
                };
            }
            catch (Exception ex)
            {
                if (ex is BusinessLogicException)
                {
                    var detailCode = ((BusinessLogicException)ex).Detail.DetailCode;

                    throw new ApiException() { ErrorCode = detailCode };
                }

                throw;
            }
        }

        [HttpGet]
        public GatewayRegisterResultModel Replace(GatewayRegisterModel model)
        {
            var gateway = new GatewayDto()
            {
                Name = model.BoxName,
                Mac = model.BoxMac
            };

            try
            {
                gateway = this.clientService.ReplaceGateway(gateway);

                return new GatewayRegisterResultModel()
                {
                    BoxId = gateway.UniqueId,
                    Timestamp = model.Timestamp
                };
            }
            catch (Exception ex)
            {
                if (ex is BusinessLogicException)
                {
                    var detailCode = ((BusinessLogicException)ex).Detail.DetailCode;

                    throw new ApiException() { ErrorCode = detailCode };
                }

                throw;
            }
        }
    }
}