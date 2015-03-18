using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UomConversionDto
    {
        public long CommodityId { get; set; }

        public long SourceUomId { get; set; }

        public long DestinationUomId { get; set; }

        public decimal? ConversionFactor { get; set; }
    }
}
