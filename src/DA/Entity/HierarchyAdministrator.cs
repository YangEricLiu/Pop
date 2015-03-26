using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Pop.Entity.Enumeration;

namespace SE.DSP.Pop.Entity
{
    public class HierarchyAdministrator
    {
        public HierarchyAdministrator()
        {
        }

        public HierarchyAdministrator(long hierarchyId, string name, string title, string telephone, string email)
        {
            this.HierarchyId = hierarchyId;
            this.Name = name;
            this.Title = title;
            this.Telephone = telephone;
            this.Email = email;
        }

        public long Id { get; set; }

        public long HierarchyId { get; set; }
 
        public string Name { get; set; }

        public string Title { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }
    }
}
