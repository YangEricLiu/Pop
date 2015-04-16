using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class SceneLog
    {
        public SceneLog()
        {
        }

        public SceneLog(long hierarchyId, string content, string createUser)
        {
            this.HierarchyId = hierarchyId;
            this.Content = content;
            this.CreateUser = createUser;
            this.CreateTime = DateTime.Now;
        }

        public long Id { get; set; }

        public long HierarchyId { get; set; }

        public string Content { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
