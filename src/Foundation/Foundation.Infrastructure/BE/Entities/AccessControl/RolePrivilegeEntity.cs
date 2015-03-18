using SE.DSP.Foundation.Infrastructure.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class RolePrivilegeEntity : EntityBase
    {
        public long RoleId { get; set; }
        public String PrivilegeCode { get; set; }
    }
}
