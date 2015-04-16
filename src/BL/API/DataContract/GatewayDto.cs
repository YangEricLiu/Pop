using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class GatewayDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Mac { get; set; }

        [DataMember]
        public string UniqueId { get; set; }

        [DataMember]
        public long CustomerId { get; set; }

        [DataMember]
        public DateTime RegisterTime { get; set; }

        [DataMember]
        public long? HierarchyId { get; set; }

        public bool IsGatewayNameValid()
        {
            if (!string.IsNullOrEmpty(this.Name) && this.Name.Contains("."))
            {
                var segements = this.Name.Split('.');

                if (segements.Length == 2)
                {
                    var customerCode = segements.FirstOrDefault();
                    var boxName = segements.LastOrDefault();

                    if (!string.IsNullOrEmpty(customerCode) && !string.IsNullOrEmpty(boxName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsGatewayMacValid()
        {
            if (!string.IsNullOrEmpty(this.Mac) && this.Mac.Length == 12)
            {
                return true;
            }

            return false;
        }
    }
}
