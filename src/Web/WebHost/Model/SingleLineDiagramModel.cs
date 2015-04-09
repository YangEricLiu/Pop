using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class SingleLineDiagramModel
    {  
        public long? Id { get; set; }

        public long HierarchyId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }

        public byte[] Content { get; set; }
    }
}
