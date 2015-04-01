using System.ServiceModel;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IPopClientService
    {
        [OperationContract]
        GatewayDto[] GetGatewayByCustomerId(long customerId);
        
        [OperationContract]
        GatewayDto RegisterGateway(GatewayDto gateway);

        [OperationContract]
        GatewayDto ReplaceGateway(GatewayDto gateway);
    }
}
