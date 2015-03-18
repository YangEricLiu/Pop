using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class VariableItem
    {
        public String Name { get; set; }
        public String Code { get; set; }
        public VariableItemType Type { get; set; }
        public String HierarchyName { get; set; }
        public String HierarchyCode { get; set; }
        public long? UomId { get; set; }
        public long? CommodityId { get; set; }
    }
}
