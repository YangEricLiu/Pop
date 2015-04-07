using System.ServiceModel;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IDistributionCabinetService
    {
        [OperationContract]
        DistributionCabinetDto GetDistributionCabinetById(long hierarchyId);

        [OperationContract]
        DistributionCabinetDto CreateDistributionCabinet(DistributionCabinetDto park);

        [OperationContract]
        DistributionCabinetDto UpdateDistributionCabinet(DistributionCabinetDto park);

        [OperationContract]
        void DeleteDistributionCabinet(long hierarchyId);
    }
}
