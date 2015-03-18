
using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    public class SPConfigChangedFilter : FilterBase
    {
        public DateTime? ChangedDate { get; set; }
        public CalcPriority? Priority { get; set; }
    }
}
