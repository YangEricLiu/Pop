using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API.ErrorCode
{
    public static class HierarchyError
    {
        public const int HierarchyHasNoParent = 001;
        public const int HierarchyCodeDuplicate = 002;
        public const int HierarchyNameDuplicate = 003;
        public const int OrganizationNestingOverLimitation = 004;
    }
}
