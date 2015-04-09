using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class DistributionRoom
    {
        public DistributionRoom()
        {
        }

        public DistributionRoom(long hierarchyId, string location, int? level, long? transformerVoltage)
        {
            this.HierarchyId = hierarchyId;
            this.Location = location;
            this.Level = level;
            this.TransformerVoltage = transformerVoltage;
        }

        public long HierarchyId { get; set; }

        public string Location { get; set; }

        public int? Level { get; set; }

        public long? TransformerVoltage { get; set; }
    }
}
