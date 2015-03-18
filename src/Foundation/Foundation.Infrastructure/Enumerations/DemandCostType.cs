/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: DemandCostType.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Demand cost type
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Demand Type for Cost
    /// </summary>
    public enum DemandCostType
    {
        /// <summary>
        /// Nothing
        /// </summary>
        None = 0,

        /// <summary>
        /// Transformer Type
        /// </summary>
        Transformer = 1,

        /// <summary>
        /// Hour Type
        /// </summary>
        Hour = 2
    }
}
