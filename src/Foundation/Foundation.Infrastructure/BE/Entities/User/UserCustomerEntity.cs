using SE.DSP.Foundation.Infrastructure.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserCustomerEntity:EntityBase
    {
        public long UserId { get; set; }
        public long CustomerId { get; set; }
        public bool WholeCustomer { get; set; }
    }
}
