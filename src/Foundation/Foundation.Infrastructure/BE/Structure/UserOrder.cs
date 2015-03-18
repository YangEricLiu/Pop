using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public struct UserOrder
    {
        public Order[] Orders { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Start { get; set; }

        public struct Order
        {
            public UserColumn Column;
            public OrderType Type;
        }
    }

    public enum UserColumn
    {
        Name = 0,
    }
}
