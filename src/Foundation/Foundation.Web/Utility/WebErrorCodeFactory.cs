using System;


using SE.DSP.Foundation.Infrastructure.Utils;
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// The web layer error code factory class.
    /// </summary>
    public static class WebErrorCodeFactory
    {
        /// <summary>
        /// Get success error.
        /// </summary>
        /// <returns>The string of <see cref="ErrorCode.Success" /></returns>
        public static RemError GetSuccessError()
        {
            return new RemError()
                       {
                           Code = Convert.ToInt32(ErrorCode.Success).ToString()
                       };
        }

        /// <summary>
        /// Get failing error.
        /// </summary>
        /// <returns>The string of <see cref="ErrorCode.Failing" /></returns>
        public static RemError GetFailingError()
        {
            return new RemError()
            {
                Code = Convert.ToInt32(ErrorCode.Failing).ToString()
            };
        }

        /// <summary>
        /// Get no funcation privilege error.
        /// </summary>
        /// <returns>The string of <see cref="ErrorCode.NoFunctionPrivilege" /></returns>
        public static RemError GetNoFuncationPrivilegeError()
        {
            return new RemError()
                       {
                           Code = Convert.ToInt32(ErrorCode.NoFunctionPrivilege).ToString()
                       };
        }

        /// <summary>
        /// Get no data privilege error.
        /// </summary>
        /// <returns>The string of <see cref="ErrorCode.NoDataPrivilege" /></returns>
        public static RemError GetNoDataPrivilegeError()
        {
            return new RemError()
            {
                Code = Convert.ToInt32(ErrorCode.NoDataPrivilege).ToString()
            };
        }

        /// <summary>
        /// Get full error code for specified error code, <see cref="ErrorType" />, web layer and <see cref="Module" />.
        /// </summary>
        /// <param name="errorCode">The detail error code.</param>
        /// <param name="errorType">The errorType.</param>
        /// <param name="module">The module where the error occured.</param>
        /// <returns>The full error code string.</returns>
        public static string GetWebErrorCode(int errorCode, ErrorType errorType, Module module)
        {
            return ErrorCodeFactory.GetErrorCode(errorCode, errorType, Layer.Web, module);
        }
    }
}