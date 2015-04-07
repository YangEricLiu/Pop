using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class OrganizationModel : BaseHierarchyModel
    {  
        public GatewayModel[] Gateways { get; set; }

        public HierarchyAdministratorModel[] Administrators { get; set; }
    }
}