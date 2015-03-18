using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
  
    /// <summary>
    /// TagFilter
    /// </summary>
    public class TagFilter : FilterBase
    {
        /// <summary>
        /// Ids
        /// </summary>
        public long[] Ids { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string LikeCode { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string LikeName { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// MeterCode
        /// </summary>
        public string MeterCode { get; set; }
        /// <summary>
        /// ChannelId
        /// </summary>
        public bool? DayNightRatio { get; set; }
        /// <summary>
        /// ChannelId
        /// </summary>
        public long? ChannelId { get; set; }
        /// <summary>
        /// TagType
        /// </summary>
        public TagType[] Type { get; set; }

        /// <summary>
        /// UomId
        /// </summary>
        public long? UomId { get; set; }
        /// <summary>
        /// CommodityId
        /// </summary>
        public long? CommodityId { get; set; }
        /// <summary>
        /// EnergyConsumptionFlag
        /// </summary>
        public EnergyConsumptionFlag[] EnergyConsumptionFlag { get; set; }

        /// <summary>
        /// Association
        /// </summary>
        public Association Association { get; set; }
        /// <summary>
        /// CalculationType
        /// </summary>
        public CalculationType? CalculationType { get; set; }
        /// <summary>
        /// IgnoreDataAuth
        /// </summary>
        public bool? ContainsUnassociated { get; set; }
    }
}
