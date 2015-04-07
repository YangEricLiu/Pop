using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class OrganizationDto : BaseHierarchyDto
    { 
        [DataMember]
        public HierarchyAdministratorDto[] Administrators { get; set; }

        [DataMember]
        public GatewayDto[] Gateways { get; set; }
    }
}
