using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System.ComponentModel.DataAnnotations;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class HierarchyModel : BaseHierarchyModel
    {
        public HierarchyModel[] Children { get; set; }

        public string Comment { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public long SpId { get; set; }

        [Required]
        public HierarchyType Type { get; set; }

        public long IndustryId { get; set; }

        public long ZoneId { get; set; }

        public long Version { get; set; }

        public string Path { get; set; }
    }
}