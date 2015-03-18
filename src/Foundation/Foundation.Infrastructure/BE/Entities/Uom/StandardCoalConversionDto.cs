using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class StandardCoalConversionDto
    {
        public long SpId { get; set; }
        public long CommodityId { get; set; }
        public long UomId { get; set; }
        public Dictionary<int, decimal?> ConversionFactors { get; set; }
    }
}
