using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BaseClass;

using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class DataPrivilegeDto: DtoBase
    {
        public bool WholeSystem { get; set; }
        public long UserId { get; set; }
        public DataAuthType PrivilegeType { get; set; }
        public CustomerDataPrivilege[] Privileges { get; set; }
    }
    
    public class CustomerDataPrivilege
    {
        public long CustomerId { get; set; }
        public bool WholeCustomer { get; set; }
        public String CustomerName { get; set; }
        public bool? Privileged { get; set; }
        public long[] HierarchyIds { get; set; } 
    }
}
