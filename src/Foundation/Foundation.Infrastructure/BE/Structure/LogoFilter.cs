using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    /// <summary>
    /// Class for filtering temp logo items
    /// </summary>
    public class LogoFilter
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Hierarchy
        /// </summary>
        public long? HierarchyId { get; set; }
        /// <summary>
        /// UpdateTimeFrom
        /// </summary>
        public DateTime? UpdateTimeFrom{ get; set; }
        /// <summary>
        /// UpdateTimeTo
        /// </summary>
        public DateTime? UpdateTimeTo { get; set; }
    }
}
