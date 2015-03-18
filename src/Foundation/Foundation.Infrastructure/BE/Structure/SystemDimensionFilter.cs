using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class SystemDimensionFilter
    {
        public long[] CustomerIds { get; set; }
        public long[] HierarchyIds { get; set; }
        public long[] SystemDimensionIds { get; set; }
        public HierarchyType[] HierarchyTypes { get; set; }
    }
    public class SystemDimensionTemplateItemFilter
    {
        public long[] CustomerIds { get; set; }
        public String[] SystemDimensionTemplateItemCodes { get; set; }
    }
}
