using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class RoleFilter
    {
        public long? ExcludeId { get; set; }
        public String Name { get; set; }
        public long[] UserIds { get; set; }
        public long[] RoleIds { get; set; }
    }
}
