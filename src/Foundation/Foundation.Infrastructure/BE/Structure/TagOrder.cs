using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Enumerations;
namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{


    public struct TagOrder
    {
        public Order[] Orders { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Start { get; set; }

        public struct Order
        {
            public TagColumn Column;
            public OrderType Type;
        }
    }
}
