using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    /// <summary>
    /// Class for filtering variable items
    /// </summary>
    public class CustomerFilter
    {
        public long? CustomerId { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public long? ExcludeId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public long? UserId { get; set; }
    }
}
