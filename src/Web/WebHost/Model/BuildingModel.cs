using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class BuildingModel
    {
        public long? HierarchyId { get; set; }

        public string Name { get; set; }

        public decimal BuildingArea { get; set; }

        public DateTime FinishingDate { get; set; }

        public LogoModel Logo { get; set; }

        public BuildingLocationModel Location { get; set; }

        public HierarchyAdministratorModel[] Administrators { get; set; }
    }
}
