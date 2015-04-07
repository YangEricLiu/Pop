using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class DistributionRoomDto : BaseHierarchyDto
    {
        [DataMember]
        public long TransformerVoltage { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public HierarchyAdministratorDto[] Administrators { get; set; }

        [DataMember]
        public GatewayDto[] Gateways { get; set; }
    }
}
