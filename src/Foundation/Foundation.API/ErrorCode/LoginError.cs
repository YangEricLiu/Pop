using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.API.ErrorCode
{
    public static class LoginError
    {
        public const int UserDoesNotExist = 001;
        public const int UserSpIsInvalidate = 002;
        public const int UserSpIsDeleted = 003;
        public const int UserSpIsInProcessing = 004;
        public const int UserIsInactive = 005;

        public const int PasswordIsIncorrect = 006;

        public const int SpDomainIsIncorrect = 007;
    }
}
