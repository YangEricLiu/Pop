using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API.ErrorCode
{
    public static class HierarchyError
    {
        public const int HierarchyNameIsEmpty = 001;
        public const int HierarchyCodeIsEmpty = 002;

        public const int HierarchyHasNoParent = 003;
        public const int HierarchyCodeDuplicate = 004;
        public const int HierarchyNameDuplicate = 005;
        public const int OrganizationNestingOverLimitation = 006;

        public const int HierarchyHasChildren = 007;
    }
}
