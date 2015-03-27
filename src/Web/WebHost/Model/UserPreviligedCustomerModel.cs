using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class UserPreviligedCustomerModel
    {
        public long CustomerId { get; set; }

        public string CustomerName { get; set; }

        public long? CustomerLogoId { get; set; }
    }
}