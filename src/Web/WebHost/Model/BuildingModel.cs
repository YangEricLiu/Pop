using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class BuildingModel : BaseHierarchyModel
    { 
        public decimal BuildingArea { get; set; }

        public long IndustryId { get; set; }

        public DateTime FinishingDate { get; set; }

        public LogoModel Logo { get; set; }

        public BuildingLocationModel Location { get; set; }

        public SingleLineDiagramModel[] SingleLineDiagrams { get; set; }

        public HierarchyAdministratorModel[] Administrators { get; set; } 
    }
}
