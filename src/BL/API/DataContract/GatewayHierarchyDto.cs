using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API.DataContract
{
    public enum GatewayHierarchyType
    {
        Cabinet = 1,
        Device = 2,
        Parameter = 3,
    }

    [DataContract]
    public class GatewayHierarchyDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public long ParentId { get; set; }

        [DataMember]
        public GatewayHierarchyType Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public GatewayHierarchyDto[] Children { get; set; }
    }
}
