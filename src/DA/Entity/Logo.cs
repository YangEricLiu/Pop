using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class Logo
    {
        public Logo()
        {
        }

        public Logo(long hierarchyId)
        {
            this.HierarchyId = hierarchyId;
        }

        public long Id { get; set; }

        public long HierarchyId { get; set; }
    }
}
