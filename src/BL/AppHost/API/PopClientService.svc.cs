using System;
using System.Linq;
using Microsoft.Practices.Unity;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.Contract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    public class PopClientService : ServiceBase, IPopClientService
    {
        private readonly IGatewayRepository gatewayRepository;

        public PopClientService()
        {
            this.gatewayRepository = IocHelper.Container.Resolve<IGatewayRepository>();
        }

        public GatewayDto[] GetGatewayByCustomerId(long customerId)
        {
            var gateways = this.gatewayRepository.GetByCustomerId(customerId);

            return gateways.Select(g => AutoMapper.Mapper.Map<GatewayDto>(g)).ToArray();
        }
    }
}
