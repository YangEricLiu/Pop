using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class UserCustomerFilter
    {
        public long[] UserIds { get; set; }
        public long[] CustomerIds { get; set; }
        public bool? WholeCustomer { get; set; }
    }
}
