using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    /// <summary>
    /// Association
    /// </summary>
    public class Association
    {
        public long HierarchyId { get; set; }

        /// <summary>
        /// Association Id
        /// </summary>        
        [Obsolete]
        public long AssociationId 
        {
            get
            {
                return (AssociationIds != null && AssociationIds.Length > 0) ? AssociationIds[0] : 0; 
            }
            set 
            {
                if (AssociationIds == null || AssociationIds.Length == 0) { AssociationIds = new long[] { value }; }
                else { AssociationIds[0] = value; }
            }
        }
        /// <summary>
        /// Association Ids
        /// </summary>
        public long[] AssociationIds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AssociationOption AssociationOption { get; set; }



        public bool? Associatiable { set; get; }
       
    }
}