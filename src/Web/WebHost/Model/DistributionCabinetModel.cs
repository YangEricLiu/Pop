using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class DistributionCabinetModel : BaseHierarchyModel
    { 
        public string Type { get; set; }

        public string Factory { get; set; }

        public DateTime? ManufactureTime { get; set; }
 
        public LogoModel Logo { get; set; }

        public SingleLineDiagramModel[] SingleLineDiagrams { get; set; }
    }
}
