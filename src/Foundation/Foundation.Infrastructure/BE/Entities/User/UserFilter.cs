using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class UserFilter
    {
        public UserTitle? Title { get; set; } 
        public long[] UserIds { get; set; }
        public long? ExcludeId { get; set; }
        public String Name { get; set; }
        public String LikeName { get; set; }
        public String RealName { get; set; }
        public String LikeRealName { get; set; }
        public long[] CustomerIds { get; set; }
        public long[] RoleIds { get; set; }
        public int? DemoStatus { get; set; }
        public long SpId { get; set; }
    }
}
