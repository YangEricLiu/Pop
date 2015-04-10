using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class BaseHierarchyModel
    {
        public long? Id { get; set; }

        public long? ParentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public int Type { get; set; }
    }
}