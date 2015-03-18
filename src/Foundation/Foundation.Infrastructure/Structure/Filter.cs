using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Structure
{
    public class Filter
    {
        public String field { get; set; }
        public String type { get; set; }
        public String[] value { get; set; }

        public const String typelist = "list";
        public const String typestring = "string";
    }
}
