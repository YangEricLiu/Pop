using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Models
{
    public class LoginModel
    {
        [Required]
       
        public string UserName { get; set; }

        [Required] 
        public string Password { get; set; }
         
        
        public string SpDomain { get; set; }
    }
}