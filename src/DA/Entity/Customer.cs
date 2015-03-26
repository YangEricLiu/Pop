﻿using System;
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

        public Customer(long hierarchyId, DateTime startTime)
        {
            this.HierarchyId = hierarchyId;
            this.StartTime = startTime;
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