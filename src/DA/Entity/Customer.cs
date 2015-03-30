using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(long hierarchyId, string address, DateTime startTime)
        {
            this.HierarchyId = hierarchyId;
            this.Address = address;
            this.StartTime = startTime;
            this.Manager = "SchneiderElectric";
            this.Telephone = "13888888888";
            this.Email = "test@schneider-electric.com";
            this.SpId = 1;
        }

        public long HierarchyId { get; set; }

        public string Address { get; set; }

        public string Manager { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public DateTime StartTime { get; set; }

        public long SpId { get; set; }
    }
}
