using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public enum GatewayRegisterResultStatus
    { 
        OK = 0,
        Rejected = -1
    }

    public class GatewayRegisterResultModel
    {
        public GatewayRegisterResultStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string BoxId { get; set; }

        public long Timestamp { get; set; }
    }
}