using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class HierarchyFilter
    {
        public long[] HierarchyIds { get; set; }
        public long? UserId { get; set; }
        public long? ParentId { get; set; }
        public long[] CustomerIds { get; set; }
        public HierarchyType? Type { get; set; }
        public String Code { get; set; }
    }
}
