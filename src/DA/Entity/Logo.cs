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

        public Logo(long hierarchyId, string udpateUser)
        {
            this.HierarchyId = hierarchyId;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = udpateUser; 
        }

        public long Id { get; set; }

        public long HierarchyId { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
