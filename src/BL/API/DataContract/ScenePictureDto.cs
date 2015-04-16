using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class ScenePictureDto
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long HierarchyId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public byte[] Content { get; set; }
    }
}
