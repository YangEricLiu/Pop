using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API.ErrorCode
{
    public static class GatewayError
    {
        public const int InvalidGatewayName = 001;
        public const int InvalidGatewayMac = 002;
        public const int GatewayCustomerNotExist = 003;
        public const int GatewayAlreadyExist = 004;
        public const int GatewayNotExist = 005;
    }
}
