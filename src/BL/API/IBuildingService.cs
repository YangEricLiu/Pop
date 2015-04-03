using System.ServiceModel;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.BL.API
{
    [ServiceContract]
    public interface IBuildingService
    {
        [OperationContract]
        BuildingDto GetBuildingById(long hierarchyId);

        [OperationContract]
        BuildingDto CreateBuilding(BuildingDto park);

        [OperationContract]
        BuildingDto UpdateBuilding(BuildingDto park);

        [OperationContract]
        void DeleteBuilding(long hierarchyId);
    }
}
