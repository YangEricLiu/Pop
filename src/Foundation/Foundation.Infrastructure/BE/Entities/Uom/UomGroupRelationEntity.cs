using System;
using SE.DSP.Foundation.Infrastructure.BaseClass;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UomGroupRelationEntity : EntityBase
    {
        public int Precision { get; set; }
        public long GroupId { get; set; }
        public long UomId { get; set; }
        public bool IsBase { get; set; }
        public bool IsCommon { get; set; }
        public bool IsStandardCoal { get; set; }
        public decimal Factor { get; set; }

        public CommodityEntity Commodity { get; set; }
        public UomEntity Uom { get; set; }
    }
}
