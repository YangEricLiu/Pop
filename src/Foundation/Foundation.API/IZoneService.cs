using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System.ServiceModel;

namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface IZoneService
    {
        [OperationContract]
        ZoneDto GetZoneById(long zoneId);

        [OperationContract]
        ZoneDto[] GetAllZones(bool includeRoot);
    }
}