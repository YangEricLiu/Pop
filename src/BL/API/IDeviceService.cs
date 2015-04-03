using System.ServiceModel;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IDeviceService
    {
        [OperationContract]
        DeviceDto GetDeviceById(long hierarchyId);

        [OperationContract]
        DeviceDto CreateDevice(DeviceDto park);

        [OperationContract]
        DeviceDto UpdateDevice(DeviceDto park);

        [OperationContract]
        void DeleteDevice(long hierarchyId);
    }
}
