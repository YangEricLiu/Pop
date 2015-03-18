using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
  
    /// <summary>
    /// TagFilter
    /// </summary>
    public class TagDAGFilter
    {
        public long[] StartVertexes { get; set; }
        public long[] EndVertexes { get; set; }
        public DAGReferType? ReferType { get; set; }
    }
}
