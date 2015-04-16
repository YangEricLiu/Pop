using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class SceneLog
    {
        public long Id { get; set; }

        public long HierarchyId { get; set; }

        public string Content { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
