using System;
using SE.DSP.Foundation.Infrastructure.BaseClass;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UomGroupEntity : EntityBase
    {
        public long CommodityId { get; set; }
        public bool IsEnergyConsumption { get; set; }
    }
}
