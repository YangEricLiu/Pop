

using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class TBConfigChangedFilter : FilterBase
    {
        public long? TBId { get; set; }
        public long? HierarchyId { get; set; }
        public long? SystemDimensionId { get; set; }
        public long? AreaDimensionid { get; set; }
        public ConfigChangedTarget? Target { get; set; }
        public DateTime? ChangedDate { get; set; }
        public CalcPriority? Priority { get; set; }
    }
}
