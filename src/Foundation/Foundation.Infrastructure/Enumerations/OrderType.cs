/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: OrderType.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : asc/desc
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Order type when query data from OTS.
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Ascending order
        /// </summary>
        ASC = 0,

        /// <summary>
        /// Descending order.
        /// </summary>
        DESC = 1,
    }
}