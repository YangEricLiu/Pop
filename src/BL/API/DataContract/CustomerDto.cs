using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class CustomerDto : BaseHierarchyDto
    {
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Manager { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public LogoDto Logo { get; set; }

        [DataMember]
        public HierarchyAdministratorDto[] Administrators { get; set; }
    }
}
