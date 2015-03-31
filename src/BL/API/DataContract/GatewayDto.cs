using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class GatewayDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Mac { get; set; }

        [DataMember]
        public DateTime RegisterTime { get; set; }

        [DataMember]
        public long? HierarchyId { get; set; }

        [DataMember]
        public string UniqueId { get; set; }

        [DataMember]
        public long CustomerId { get; set; }
    }
}
