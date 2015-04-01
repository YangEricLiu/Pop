using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Common.Exceptions
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }
    }
}