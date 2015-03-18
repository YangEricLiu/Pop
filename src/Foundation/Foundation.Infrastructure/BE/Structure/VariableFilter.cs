using SE.DSP.Foundation.Infrastructure.BaseClass;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    /// <summary>
    /// Class for filtering variable items
    /// </summary>
    public class VariableFilter : FilterBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public String LikeName { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public String LikeCode { get; set; }

        /// <summary>
        /// VariableType, please refer to <see cref="VariableType"/>
        /// </summary>
        public VariableItemType[] Type { get; set; }

        /// <summary>
        /// HierarchyName
        /// </summary>
        public String HierarchyName { get; set; }

        /// <summary>
        /// HierarchyCode
        /// </summary>
        public String HierarchyCode { get; set; }

        /// <summary>
        /// HierarchyName
        /// </summary>
        public String HierarchyLikeName { get; set; }

        /// <summary>
        /// HierarchyCode
        /// </summary>
        public String HierarchyLikeCode { get; set; }

        /// <summary>
        /// UomId
        /// </summary>
        public long? UomId { get; set; }

        /// <summary>
        /// CommodityId
        /// </summary>
        public long? CommodityId { get; set; }
        /// <summary>
        /// UomId
        /// </summary>
        public bool? DayNightRatio { get; set; }
        ///// <summary>
        ///// IgnoreDataAuth
        ///// </summary>
        //public bool? IgnoreDataAuth { get; set; }
    }
}
