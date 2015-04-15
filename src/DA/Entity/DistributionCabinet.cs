using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class DistributionCabinet
    {
        public DistributionCabinet()
        {
        }

        public DistributionCabinet(long hierarchyId, long? gatewayId, string type, string factory, DateTime? manufactureTime)
        {
            this.HierarchyId = hierarchyId;
            this.GatewayId = gatewayId;
            this.Type = type;
            this.Factory = factory;
            this.ManufactureTime = manufactureTime;
        }

        public long HierarchyId { get; set; }

        public long? GatewayId { get; set; }

        public string Type { get; set; }

        public string Factory { get; set; }

        public DateTime? ManufactureTime { get; set; }
    }
}
