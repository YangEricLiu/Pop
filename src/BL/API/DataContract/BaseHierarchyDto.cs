﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public abstract class BaseHierarchyDto
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long ParentId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public HierarchyType Type { get; set; }
    }
}
