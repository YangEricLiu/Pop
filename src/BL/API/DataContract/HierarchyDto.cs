using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class HierarchyDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public HierarchyDto[] Children { get; set; }
    }
}
