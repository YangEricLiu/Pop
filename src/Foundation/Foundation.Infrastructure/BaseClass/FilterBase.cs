using SE.DSP.Foundation.Infrastructure.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BaseClass
{
    public class FilterBase
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long? AuthUserId { get; set; }

        /// <summary>
        /// ExcludeId
        /// </summary>
        public long? ExcludeId { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// StatusFilter
        /// </summary>
        public StatusFilter StatusFilter { get; set; }
    }
}
