using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    /// <summary>
    /// FormulaDAGInfo
    /// </summary>
    public class AreaDimensionFilter
    {
        public long[] CustomerIds { get; set; }
        public long[] HierarchyIds { get; set; }
        //public long? AncestorHierarchyId { get; set; }
        public long[] AreaDimensionIds { get; set; }
        //public long? AncestorAreaDimensionId { get; set; }
    }
}
