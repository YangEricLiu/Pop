using System.ServiceModel;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IParkService
    {
        [OperationContract]
        ParkDto GetParkById(long hierarchyId);

        [OperationContract]
        ParkDto CreatePark(ParkDto park);

        [OperationContract]
        ParkDto UpdatePark(ParkDto park);

        [OperationContract]
        void DeletePark(long hierarchyId);
    }
}
