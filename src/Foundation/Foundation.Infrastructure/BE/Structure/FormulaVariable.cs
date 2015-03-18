using SE.DSP.Foundation.Infrastructure.Expr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
   

    /// <summary>
    /// FormulaVariable
    /// </summary>
    public class FormulaVariable
    {
        /// <summary>
        /// VariableType
        /// </summary>
        public VariableType VariableType { get; set; }
        /// <summary>
        /// HierarchyCode
        /// </summary>
        public String HierarchyCode { get; set; }
        /// <summary>
        /// VariableCode
        /// </summary>
        public String VariableCode { get; set; }
        /// <summary>
        /// CustomerId
        /// </summary>
        public long CustomerId { get; set; }
    }
}
