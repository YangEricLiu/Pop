using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class HierarchyAdministratorModel
    {
        public long? Id { get; set; }

        public long? HierarchyId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }
    }
}