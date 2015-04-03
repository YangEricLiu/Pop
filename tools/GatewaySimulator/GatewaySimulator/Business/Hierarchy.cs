using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator
{
    public class Hierarchy
    {
        private string Version = ConfigurationManager.AppSettings["MessageVersion"];
        private string PushTopicFormat = "{version}/hpush/{boxid}";
V1.0/hcack/{boxid}
V1.0/hget/{boxid}

    }
}
