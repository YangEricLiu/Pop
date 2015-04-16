using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class DeviceDto : BaseHierarchyDto
    { 
        [DataMember]
        public string Factory { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public long? GatewayId { get; set; }

        [DataMember]
        public LogoDto Logo { get; set; }

        [DataMember]
        public SceneLogDto[] SceneLogs { get; set; }

        [DataMember]
        public ScenePictureDto[] ScenePictures { get; set; }
    }
}
