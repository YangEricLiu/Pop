using System.Linq;
using SE.DSP.Pop.BL.API;
using SE.DSP.Pop.BL.API.DataContract;
using SE.DSP.Pop.BL.AppHost.Common.Ioc;
using SE.DSP.Pop.Contract;

namespace SE.DSP.Pop.BL.AppHost.API
{
    [IocServiceBehavior]
    public class PopClientService : IPopClientService
    {
        private readonly IGatewayRepository gatewayRepository;

        public PopClientService(IGatewayRepository gatewayRepository)
        {
            this.gatewayRepository = gatewayRepository;
        }

        public GatewayDto[] GetGatewayByCustomerId(long customerId)
        {
            var gateways = this.gatewayRepository.GetByCustomerId(customerId);

            return gateways.Select(g => AutoMapper.Mapper.Map<GatewayDto>(g)).ToArray();
        }
    }
}
