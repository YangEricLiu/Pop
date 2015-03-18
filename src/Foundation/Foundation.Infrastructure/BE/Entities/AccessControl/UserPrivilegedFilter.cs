using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserPrivilegedFilter
    {
        public long ExcludeUserId { get; set; }
        public DataAuthType PrivilegeType { get; set; }
        public long PrivileteItemId { get; set; }
    }
}
