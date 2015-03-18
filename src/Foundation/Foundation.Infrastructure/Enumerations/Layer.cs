/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: Layer.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Layer enum
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Layer, each has 1 number.
    /// </summary>
    public enum Layer
    {
        /// <summary>
        /// DA layer.
        /// </summary>
        DA = 1,

        /// <summary>
        /// BL layer.
        /// </summary>
        BL = 2,

        /// <summary>
        /// Web layer.
        /// </summary>
        Web = 3,

        /// <summary>
        /// Client layer.
        /// </summary>
        Client = 4,

        /// <summary>
        /// Common layer.
        /// </summary>
        Common = 9
    }
}