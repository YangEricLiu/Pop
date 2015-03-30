using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class SaveUserCustomerModel
    {
        public long UserId { get; set; }

        public UserCustomerModel[] UserCustomers { get; set; }
    }
}