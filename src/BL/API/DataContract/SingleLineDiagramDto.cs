using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class SingleLineDiagramDto
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long HierarchyId { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public string CreateUser { get; set; }

        [DataMember]
        public DateTime? UpdateTime { get; set; }

        [DataMember]
        public string UpdateUser { get; set; }

        [DataMember]
        public byte[] Content { get; set; }
    }
}
