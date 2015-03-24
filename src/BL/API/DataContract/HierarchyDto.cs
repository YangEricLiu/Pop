using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class HierarchyDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public HierarchyType Type { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long TimezoneId { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public long? ParentId { get; set; }

        [DataMember]
        public long CustomerId { get; set; }

        //public string Path { get; set; }

        [DataMember]
        public int PathLevel { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public long? IndustryId { get; set; }

        [DataMember]
        public long? ZoneId { get; set; }

        [DataMember]
        public bool CalcStatus { set; get; }

        [DataMember]
        public long SpId { get; set; }

        [DataMember]
        public string UpdateUser { get; set; }

        [DataMember]
        public DateTime? UpdateTime { get; set; }

        //public long Version { get; set; }

        [DataMember]
        public HierarchyDto[] Children { get; set; }
    }
}
