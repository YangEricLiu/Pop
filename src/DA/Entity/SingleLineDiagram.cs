using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class SingleLineDiagram
    {
        public SingleLineDiagram()
        {
        }

        public SingleLineDiagram(long hierarchyId, string key, int order, string createUser)
        {
            this.HierarchyId = hierarchyId;
            this.Key = key;
            this.Order = order;
            this.CreateTime = DateTime.Now;
            this.CreateUser = createUser;
        }

        public long Id { get; set; }

        public long HierarchyId { get; set; }

        public string Key { get; set; }

        public int Order { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
