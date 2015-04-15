using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class DistributionCabinetDto : BaseHierarchyDto
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Factory { get; set; }

        [DataMember]
        public long? GatewayId { get; set; }

        [DataMember]
        public DateTime? ManufactureTime { get; set; }

        [DataMember]
        public LogoDto Logo { get; set; }

        [DataMember]
        public SingleLineDiagramDto[] SingleLineDiagrams { get; set; }
    }
}
