
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class TagConfigChangedFilter
    {
        public long? TagId { get; set; }
        public ConfigChangedTarget? Target { get; set; }
        public DateTime? ChangedDate { get; set; }
        public CalcPriority? Priority { get; set; }
        /// <summary>
        /// StatusFilter
        /// </summary>
        public StatusFilter StatusFilter { get; set; }
    }
}
