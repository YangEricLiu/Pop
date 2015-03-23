using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.BL.API.DataContract
{
    public class UserDto
    { 
        public long? Id { get; set; }

        public String RealName { get; set; }

        public long UserType { get; set; }

        public String UserTypeName { get; set; }
      
        public String Password { get; set; }
      
        public long[] CustomerIds { get; set; }

        public string Title { get; set; }

        public String Telephone { get; set; }
      
        public String Email { get; set; }
        public String Comment { get; set; }
        public String Name { get; set; }
        public int DemoStatus { get; set; }
        public long SpId { get; set; }
        public EntityStatus SpStatus { get; set; }
 
        public long? Version { get; set; }
    }
}
