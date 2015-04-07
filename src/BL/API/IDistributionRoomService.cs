using System.ServiceModel;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IDistributionRoomService
    {
        [OperationContract]
        DistributionRoomDto GetDistributionRoomById(long hierarchyId);

        [OperationContract]
        DistributionRoomDto CreateDistributionRoom(DistributionRoomDto park);

        [OperationContract]
        DistributionRoomDto UpdateDistributionRoom(DistributionRoomDto park);

        [OperationContract]
        void DeleteDistributionRoom(long hierarchyId);
    }
}
