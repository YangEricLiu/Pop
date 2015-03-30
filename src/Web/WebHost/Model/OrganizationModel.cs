using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class OrganizationModel
    { 
        public long? HierarchyId { get; set; }
        public string Name { get; set; }
        public HierarchyAdministratorModel[] Administrators { get; set; }
    }
}