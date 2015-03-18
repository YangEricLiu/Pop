using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Enumerations.Energy
{

    public class PagingOrder
    {
        public int PageSize { get; set; }
        public int PageIdx { get; set; }
        public Order Order { get; set; }
        public DateTime? PreviousEndTime { get; set; }
        public RequestOperation Operation { get; set; }
    }
    public class Order
    {
        public Column Column;
        public OrderType Type;
    }
    public enum Column
    {
        TIME = 1,
    }
    public enum RequestOperation
    {
        Initialize = 0,  //need to remove caches from AppFabric
        FirstTime = 1,   //need to load data from Ots then add to AppFabric
        Others = 2,     //get from Cache directly
        SkipPaging = 3
    }
}
