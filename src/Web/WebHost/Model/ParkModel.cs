using System.Runtime.Serialization;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class ParkModel : BaseHierarchyModel
    { 
        public decimal FloorSpace { get; set; }

        public decimal BuildingArea { get; set; }

        public LogoModel Logo { get; set; }

        public BuildingLocationModel Location { get; set; }

        public HierarchyAdministratorModel[] Administrators { get; set; }

        public GatewayModel[] Gateways { get; set; }
    }
}
