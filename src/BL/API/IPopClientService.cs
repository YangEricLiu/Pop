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

        [OperationContract]
        SingleLineDiagramDto GetSingleLineDiagramById(long id);

        [OperationContract]
        SingleLineDiagramDto UpdateSingleLineDiagram(SingleLineDiagramDto dto);

        [OperationContract]
        SingleLineDiagramDto CreateSingleLineDiagram(SingleLineDiagramDto dto);

        [OperationContract]
        SingleLineDiagramDto[] GetSingleLineDiagramByHierarchyId(long hierarchyId);

        [OperationContract]
        ScenePictureDto[] UploadScenePicture(long hierarchyId, ScenePictureDto[] scenePictures);

        [OperationContract]
        void DeleteSingleLineDiagramById(long id);

        [OperationContract]
        void SaveGatewayHierarchy(string boxId, long timestamp, GatewayHierarchyDto[] gatewayHierarchy);
    }
}
