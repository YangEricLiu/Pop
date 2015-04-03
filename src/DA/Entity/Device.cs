using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class Device
    {
        public Device()
        {
        }

        public Device(long hierarchyId, long? gatewayId, string factory, string description)
        {
            this.HierarchyId = hierarchyId;
            this.GatewayId = gatewayId;
            this.Factory = factory;
            this.Description = description;
        }

        public long HierarchyId { get; set; }

        public long? GatewayId { get; set; }

        public string Factory { get; set; }

        public string Description { get; set; }
    }
}
