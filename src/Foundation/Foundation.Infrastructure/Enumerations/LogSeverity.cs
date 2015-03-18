/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: LoggingSeverity.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Logging severity
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Logging severity.
    /// </summary>
    public enum LoggingSeverity
    {
        /// <summary>
        /// Log nothing.
        /// </summary>
        Off,

        /// <summary>
        /// Log fatal message.
        /// </summary>
        Fatal,

        /// <summary>
        /// Log error message.
        /// </summary>
        Error,

        /// <summary>
        /// Log warning message.
        /// </summary>
        Warning,

        /// <summary>
        /// Log information message.
        /// </summary>
        Information,

        /// <summary>
        /// Log the process of method invoking、input/output arguments、return value.
        /// </summary>
        Debug
    }
}