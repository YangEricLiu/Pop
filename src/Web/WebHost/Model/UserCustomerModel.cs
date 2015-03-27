using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class UserCustomerModel
    {
        public long HierarchyId { get; set; }

        public string CustomerName { get; set; }

        public bool WholeCustomer { get; set; }

        public bool IsAuthorized { get; set; }
    }
}