using SE.DSP.Foundation.Infrastructure.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class PTagGatewayFilter
    {
        public long ChannelId { get; set; }
        public String MeterCode { get; set; }
        public String CustomerCode { get; set; }
        public StatusFilter StatusFilter { get; set; }
    }
}
