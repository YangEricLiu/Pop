using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class ScenePicture
    {
        public ScenePicture()
        {
        }

        public ScenePicture(long hierarchyId, int order, string description, string createUser)
        {
            this.HierarchyId = hierarchyId;
            this.Order = order;
            this.Description = description;
            this.CreateTime = DateTime.Now;
            this.CreateUser = createUser;
        }

        public long Id { get; set; }

        public long HierarchyId { get; set; }

        public int Order { get; set; }

        public string Description { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
