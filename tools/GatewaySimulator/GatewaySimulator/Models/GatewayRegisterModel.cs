using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator
{
    public class GatewayRegisterModel
    {
        public long Timestamp { get; set; }

        public string BoxName { get; set; }

        public string BoxMac { get; set; }

        public string BoxId { get; set; }
    }
}
