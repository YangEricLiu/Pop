using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    /// <summary>
    /// FormulaDAGInfo
    /// </summary>
    public class FormulaDAGInfo
    {
        /// <summary>
        /// StartVertexId
        /// </summary>
        public long StartVertexId { get; set; }
        /// <summary>
        /// EndVertexId
        /// </summary>
        public String EndVertexId { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        public String Source { get; set; }
    }
}
