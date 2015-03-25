using SE.DSP.Pop.Entity.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class HierarchyModel
    {
        public long Id{get;set;}

        public HierarchyModel[] Children { get; set; }

        [Required]
        public string Code{get;set;}

        public string Comment{get;set;}

        [Required]
        public long CustomerId{get;set;}

        [Required]
        public string Name{get;set;}

        [Required]
        public long ParentId{get;set;}

        public long SpId{get;set;}

        [Required]
        public HierarchyType Type{get;set;}
        public long IndustryId{get;set;}
        public long ZoneId { get; set; }

        public long Version { get; set; }

        public string Path { get; set; }
    }
}