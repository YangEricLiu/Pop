using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class BuildingDto : BaseHierarchyDto
    {
        [DataMember]
        public long IndustryId { get; set; }

        [DataMember]
        public decimal BuildingArea { get; set; }

        [DataMember]
        public DateTime FinishingDate { get; set; }

        [DataMember]
        public LogoDto Logo { get; set; }

        [DataMember]
        public BuildingLocationDto Location { get; set; }

        [DataMember]
        public SingleLineDiagramDto[] SingleLineDiagrams { get; set; }

        [DataMember]
        public HierarchyAdministratorDto[] Administrators { get; set; }
    }
}
