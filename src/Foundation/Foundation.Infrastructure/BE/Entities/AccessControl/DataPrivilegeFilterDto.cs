using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class DataPrivilegeFilterDto
    {
        public long UserId { get; set; }
        public DataAuthType PrivilegeType { get; set; }
    }
}
