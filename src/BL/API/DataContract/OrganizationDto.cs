using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class OrganizationDto
    {
        [DataMember]
        public long? HierarchyId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public HierarchyAdministratorDto[] Administrators { get; set; }
    }
}
