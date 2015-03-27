using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class LogoModel
    {
        public long? Id { get; set; }

        public long? HierarchyId { get; set; }

        public byte[] Logo { get; set; }
    }
}