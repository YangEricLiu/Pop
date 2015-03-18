using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.Infrastructure.BaseClass;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class RoleDto : DtoBase
    {
        public String[] PrivilegeCodes { get; set; }
        public long SpId { get; set; }
    }
}
