

using SE.DSP.Foundation.Infrastructure.BE.Entities;
namespace SE.DSP.Foundation.DA.Interface
{
    public interface IZoneDA
    {
        ZoneEntity RetrieveZoneById(long zoneId);
        ZoneEntity[] RetrieveAllZones();
    }
}