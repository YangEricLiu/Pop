using System.Runtime.Serialization;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class DistributionRoomModel : BaseHierarchyModel
    { 
        public string Location { get; set; }

        public int Level { get; set; }

        public long TransformerVoltage { get; set; }
 
        public HierarchyAdministratorModel[] Administrators { get; set; }

        public SingleLineDiagramModel[] SingleLineDiagrams { get; set; }

        public GatewayModel[] Gateways { get; set; }
    }
}
