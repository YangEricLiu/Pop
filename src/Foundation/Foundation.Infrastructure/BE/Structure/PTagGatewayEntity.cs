using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{


    /// <summary>
    /// PTagGatewayEntity
    /// </summary>
    public class PTagGatewayEntity
    {
        public long Id { get; set; }
        public long ChannelId { get; set; }
        public long GuidCode { get; set; }
        //public String CustomerCode { get; set; }
        public String MeterCode { get; set; }
        public String CommodityCode { get; set; }
        public String UomCode { get; set; }
        public DateTime? StartTime { get; set; }
        public EntityStatus Status { get; set; }
    }
}
