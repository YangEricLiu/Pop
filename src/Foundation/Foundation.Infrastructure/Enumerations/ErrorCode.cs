/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Common error code
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// The common error code, each has 3 numbers.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Success.
        /// </summary>
        Success = 000,

        /// <summary>
        /// Failing.
        /// </summary>
        Failing = 001,

        /// <summary>
        /// Has no function privilege.
        /// </summary>
        NoFunctionPrivilege = 008,

        /// <summary>
        /// Has no data privilege.
        /// </summary>
        NoDataPrivilege = 009
    }
}