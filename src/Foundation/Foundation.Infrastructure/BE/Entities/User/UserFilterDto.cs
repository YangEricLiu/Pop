using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserFilterDto
    {
        public UserFilterDto()
        {
            DemoStatus = 0;
        }
        public long[] UserIds { get; set; }
        public String Name { get; set; }
        //public String LikeName { get; set; }
        //public String RealName { get; set; }
        //public String LikeRealName { get; set; }
        public String PasswordToken { get; set; }
        public long? CustomerId { get; set; }
        public long? UserType { get; set; }
        public int? DemoStatus { get; set; }
        public long? ExcludeId { get; set; }
        public long SpId { get; set; }
    }
}
