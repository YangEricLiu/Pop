using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Pop.Entity
{
    public class Park
    {
        public Park()
        {
        }

        public Park(long hierarchyId, decimal? floorSpace, decimal? buildingArea)
        {
            this.HierarchyId = hierarchyId;
            this.FloorSpace = floorSpace;
            this.BuildingArea = buildingArea;
        }

        public long HierarchyId { get; set; }

        public decimal? FloorSpace { get; set; }

        public decimal? BuildingArea { get; set; }
    }
}
