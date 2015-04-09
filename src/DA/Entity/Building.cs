using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class Building
    {
        public Building()
        {
        }

        public Building(long hierarchyId, decimal? buildingArea, DateTime? finishingdate)
        {
            this.HierarchyId = hierarchyId;
            this.BuildingArea = buildingArea;
            this.FinishingDate = finishingdate;
        }

        public long HierarchyId { get; set; } 

        public decimal? BuildingArea { get; set; }

        public DateTime? FinishingDate { get; set; }
    }
}
