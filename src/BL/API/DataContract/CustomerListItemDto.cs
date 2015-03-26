using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class CustomerListItemDto
    {
        [DataMember]
        public long CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }
    }
}
