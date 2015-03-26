using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class UserCustomerDto
    {
        [DataMember]
        public long CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public long? CustomerLogoId { get; set; }
    }
}
