using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.BaseClass;


namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class DataPrivilegeEntity: EntityBase
    {
        public long UserId { get; set; }
        public DataAuthType? PrivilegeType { get; set; }
        public long? PrivilegeItemId { get; set; }
    }
}
