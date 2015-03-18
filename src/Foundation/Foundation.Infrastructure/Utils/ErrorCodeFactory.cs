/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ErrorCodeFactory.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Factory for error code
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Configuration;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The error code factory class.
    /// </summary>
    public static class ErrorCodeFactory
    {
        /// <summary>
        /// Get success code.
        /// </summary>
        /// <returns>The string of <see cref="ErrorCode.Success" /></returns>
        public static string GetSuccessCode()
        {
            return Convert.ToInt32(ErrorCode.Success).ToString();
        }

        /// <summary>
        /// Get failing code.
        /// </summary>
        /// <returns>The string of <see cref="ErrorCode.Failing" /></returns>
        public static string GetFailingCode()
        {
            return Convert.ToInt32(ErrorCode.Failing).ToString();
        }

        /// <summary>
        /// Get full error code for specified error code, <see cref="ErrorType" />, <see cref="Layer" /> and <see cref="Module" />.
        /// </summary>
        /// <param name="errorCode">The detail error code.</param>
        /// <param name="errorType">The errorType.</param>
        /// <param name="layer">The layer where the error occured.</param>
        /// <param name="module">The module where the error occured.</param>
        /// <returns>The full error code string.</returns>
        public static string GetErrorCode(int errorCode, ErrorType errorType, Layer layer, Module module)
        {
            return Convert.ToInt32(errorType).ToString("00") + Convert.ToInt32(ConfigHelper.Get(ConfigurationKey.SERVERID)).ToString("0000") + Convert.ToInt32(layer).ToString("0") + Convert.ToInt32(module).ToString("00") + errorCode.ToString("000");
        }
    }
}