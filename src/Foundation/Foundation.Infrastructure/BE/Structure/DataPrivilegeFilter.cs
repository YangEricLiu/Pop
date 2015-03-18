using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class DataPrivilegeFilter
    {
        public long[] UserIds { get; set; }
        public long[] CustomerIds { get; set; }
        public DataAuthType? PrivilegeType { get; set; }
        public long[] PrivilegeItemIds { get; set; }
    }
}
