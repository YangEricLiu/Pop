﻿using System;
using System.Runtime.Serialization;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class UserCustomerDto
    {
        [DataMember]
        public long HierarchyId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public bool WholeCustomer { get; set; }

        [DataMember]
        public bool IsAuthorized { get; set; }
    }
}
