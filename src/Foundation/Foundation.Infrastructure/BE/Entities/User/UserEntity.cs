using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserEntity : EntityBase
    {
        public UserEntity()
        {
            DemoStatus = 0;
        }
        public String RealName { get; set; }
        public long? UserType { get; set; }
        public long[] RoleIds { get; set; }
        public long SpId { get; set; }
        public String Password { get; set; }
        public String PasswordToken { get; set; }
        public DateTime? PasswordTokenDate { get; set; }
        //public long[] RoleIds { get; set; }
        //public long[] CustomerIds { get; set; }
        public UserTitle Title { get; set; }
        public String Telephone { get; set; }
        public String Email { get; set; }
        public int DemoStatus { get; set; }
    }
}
