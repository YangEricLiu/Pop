using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class CustomerModel
    {
        public long? HierarchyId { get; set; } 
 
        public DateTime StartTime { get; set; }

        public string CustomerName { get; set; }

        public LogoModel Logo { get; set; }
 
        public HierarchyAdministratorModel[] Administrators { get; set; }
    }
}