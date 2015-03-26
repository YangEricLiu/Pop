using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class ErrorModel
    {
        public string Error { get; set; }

        public string[] Message { get; set; }

        public object Result { get; set; }
    }
}