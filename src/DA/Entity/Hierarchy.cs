using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE.DSP.Pop.Entity.Enumeration;

namespace SE.DSP.Pop.Entity
{
    public class Hierarchy 
    {
        public Hierarchy()
        {
        }

        public Hierarchy(string name, string code, HierarchyType hierarchyType)
        {
            this.Type = hierarchyType;
            this.Code = code;
            this.Name = name;
            this.TimezoneId = 1;
            this.PathLevel = 1;
            this.Status = 1;
            this.CalcStatus = true;
            this.SpId = 1;
            this.UpdateTime = DateTime.Now;
        }

        public Hierarchy(string name, string code, long parentId, HierarchyType hierarchyType) : this(name, code, hierarchyType)
        {
            this.ParentId = parentId;
        }

        public Hierarchy(string name, string code, long industryId, long parentId, HierarchyType hierarchyType)
            : this(name, code, parentId, hierarchyType)
        {
            this.IndustryId = industryId;
        }

        public long Id { get; set; }

        public HierarchyType Type { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long TimezoneId { get; set; }

        public string Comment { get; set; }

        public long? ParentId { get; set; }

        public long CustomerId { get; set; }

        public string Path { get; set; }

        public int PathLevel { get; set; }

        public int Status { get; set; }

        public long? IndustryId { get; set; }

        public long? ZoneId { get; set; }

        public bool CalcStatus { get; set; }

        public long SpId { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public long? Version { get; set; }
    }
}
