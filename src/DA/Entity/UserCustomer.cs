using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class UserCustomer
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long HierarchyId { get; set; }

        public bool WholeCustomer { get; set; }

        public DateTime UpdateUser { get; set; }

        public DateTime UpdateTime { get; set; }

        public long Version { get; set; }
    }
}
