using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class GatewayRegisterModel
    {
        public long Timestamp { get; set; }

        public string BoxName { get; set; }

        public string BoxMac { get; set; }
    }
}