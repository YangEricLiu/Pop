
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class CustomerLabellingFilter
    {
        public long? CustomerId { get; set; }
        public string Name { get; set; }
        public long? LabellingId { get; set; }
        public OrderType Order { get; set; }
    }
}
