using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class SceneLogDto
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long HierarchyId { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public string CreateUser { get; set; }
    }
}
