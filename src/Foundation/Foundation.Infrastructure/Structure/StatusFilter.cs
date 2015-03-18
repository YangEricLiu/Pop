using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Structure
{
    /// <summary>
    /// StatusFilter
    /// </summary>
    public class StatusFilter
    {
        /// <summary>
        /// Statuses
        /// </summary>
        public EntityStatus[] Statuses { get; set; }
        /// <summary>
        /// ExcludeStatus
        /// </summary>
        public bool ExcludeStatus { get; set; }
    }
}
