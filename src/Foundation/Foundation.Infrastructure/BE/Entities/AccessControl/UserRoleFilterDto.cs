using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BaseClass;


namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserRoleFilterDto:DtoBase
    {
        public long[] RoleIds { get; set; }
        public long[] UserIds { get; set; }
    }
}
