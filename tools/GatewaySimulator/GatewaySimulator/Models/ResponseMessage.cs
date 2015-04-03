using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewaySimulator
{
    public class ResponseMessage
    {
        public string Error { get; set; }

        public string[] Message { get; set; }

        public object Result { get; set; }        
    }
}
