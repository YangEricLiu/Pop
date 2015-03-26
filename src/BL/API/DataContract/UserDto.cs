using System;
using System.Runtime.Serialization;
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class UserDto
    { 
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public string RealName { get; set; }

        [DataMember]
        public long UserType { get; set; }

        [DataMember]
        public string UserTypeName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public long[] CustomerIds { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Comment { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int DemoStatus { get; set; }
        
        [DataMember]
        public long SpId { get; set; }

        [DataMember]
        public EntityStatus SpStatus { get; set; }

        [DataMember]
        public long? Version { get; set; }
    }
}
