using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class TBFilter
    {
        public long[] TagIds { get; set; }

        public string Name { get; set; }

        public TBType? TBType { get; set; }
    }
}
