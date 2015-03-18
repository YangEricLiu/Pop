
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The DA layer error code factory class.
    /// </summary>
    public static class DAErrorCodeFactory
    {
        /// <summary>
        /// Get full error code for specified error code, <see cref="ErrorType" />, DA layer and <see cref="Module" />.
        /// </summary>
        /// <param name="errorCode">The detail error code.</param>
        /// <param name="errorType">The errorType.</param>
        /// <param name="module">The module where the error occured.</param>
        /// <returns>The full error code string.</returns>
        public static string GetDAErrorCode(int errorCode, ErrorType errorType, Module module)
        {
            return ErrorCodeFactory.GetErrorCode(errorCode, errorType, Layer.DA, module);
        }
    }
}