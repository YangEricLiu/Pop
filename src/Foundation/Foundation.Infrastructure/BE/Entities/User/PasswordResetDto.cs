using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class PasswordResetDto
    {
        public String Email { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; } 
        public String PasswordToken { get; set; }
    }
}
