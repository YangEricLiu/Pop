using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class UserCustomer
    {
        public UserCustomer()
        {
        }

        public UserCustomer(long userid, long hierarchyid, bool wholeCustomer, string updateUser)
        {
            this.UserId = userid;
            this.HierarchyId = hierarchyid;
            this.WholeCustomer = wholeCustomer;
            this.UpdateTime = DateTime.Now;
            this.UpdateUser = updateUser;
        }

        public long Id { get; set; }

        public long UserId { get; set; }

        public long HierarchyId { get; set; }

        public bool WholeCustomer { get; set; }

        public string UpdateUser { get; set; }

        public DateTime UpdateTime { get; set; }

        public long Version { get; set; }

        public bool HasAllCustomer()
        {
            return this.HierarchyId == 0 && !this.WholeCustomer;
        }
    }
}
