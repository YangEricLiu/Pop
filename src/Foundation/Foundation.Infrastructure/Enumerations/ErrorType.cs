/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ErrorType.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error type enum
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Error type, each has 3 numbers.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// User operation error.
        /// </summary>
        Operation = 01,

        /// <summary>
        /// User input error.
        /// </summary>
        Input = 02,

        /// <summary>
        /// Parameter error.
        /// </summary>
        Parameter = 03,

        /// <summary>
        /// Authorization error.
        /// </summary>
        Authorization = 04,

        /// <summary>
        /// Logic error.
        /// </summary>
        Logic = 05,

        /// <summary>
        /// Concurrent error.
        /// </summary>
        Concurrent = 06,

        /// <summary>
        /// Db error.
        /// </summary>
        Database = 07,

        Custom = 99,
    }
}