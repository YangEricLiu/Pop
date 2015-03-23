using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API.DataContract
{
    [DataContract]
    public class UserDto
    { 
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public String RealName { get; set; }

        [DataMember]
        public long UserType { get; set; }

        [DataMember]
        public String UserTypeName { get; set; }

        [DataMember]
        public String Password { get; set; }

        [DataMember]
        public long[] CustomerIds { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public String Telephone { get; set; }

        [DataMember]
        public String Email { get; set; }

        [DataMember]
        public String Comment { get; set; }
        
        [DataMember]
        public String Name { get; set; }

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
