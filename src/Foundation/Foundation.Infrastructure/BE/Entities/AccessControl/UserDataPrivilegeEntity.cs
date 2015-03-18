using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserDataPrivilegeEntity:EntityBase
    {
        public long UserId { get; set; }
        public DataAuthType PrivilegeType { get; set; }
        public long[] ItemIds { get; set; }
    }
}
