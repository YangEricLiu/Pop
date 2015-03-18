using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    public enum AssociationOption
    {
        /// <summary>
        /// Unassociated to hierarchy/dimension
        /// </summary>
        NoneAssociation =0,
        /// <summary>
        /// Associated to hierarchy directly
        /// </summary>
        HierarchyOnly=1,
        /// <summary>
        /// Associated to hierarchy directly, or associated to dimensions belonging to hierarchy
        /// </summary>
        HierarchyAll=2,
        /// <summary>
        /// Associated to system dimension only, not associated to area diension
        /// </summary>
        SystemDimensionOnly = 3,
        /// <summary>
        /// Associated to system dimension, whatever if it's associated to area diension or not
        /// </summary>
        SystemDimensionAll = 4,
        /// <summary>
        /// Associated to area dimension only, not associated to system diension
        /// </summary>
        AreaDimensionOnly = 5,
        /// <summary>
        /// Associated to area dimension, whatever if it's associated to system diension or not
        /// </summary>
        AreaDimensionAll=6,

        /// <summary>
        /// Be able to be associated to hierarchy
        /// </summary>
        HierarchyAssociatiable = 7,
        /// <summary>
        /// Be able to be associated to system dimension
        /// </summary>
        SystemDimensionAssociatiable = 8,
        /// <summary>
        /// Be able to be associated to area dimension
        /// </summary>
        AreaDimensionAssociatiable=9,
        /// <summary>
        /// Exclude none associated
        /// </summary>
        ExcludeNoneAssociation = 10,


        SystemDimensionAssociatiableAll = 11,


        AreaDimensionAssociatiableAll = 12,


        /// <summary>
        /// Be able to be associated to hierarchy
        /// </summary>
        HierarchyAssociatiableAll = 13,
    }
}
