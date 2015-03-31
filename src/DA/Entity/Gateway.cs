using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class Gateway
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Mac { get; set; }

        public string UniqueId { get; set; }

        public long CustomerId { get; set; }

        public DateTime RegisterTime { get; set; }

        public long? HierarchyId { get; set; }
    }
}
