/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: AuthorizationIDAttribute.cs
 * Author	    : Figo
 * Date Created : 2011-12-01
 * Description  : Indicate the authorization ID for one Web API method 
--------------------------------------------------------------------------------------------*/

using System;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// Indicate the function code for one Web API method
    /// </summary>
    /// <remarks>If one method has been marked with this attribute, <see cref="AccessControlOperationInvoker" /> will process the authorization.</remarks>
    /// <example>
    /// <code>
    /// [OperationContract]
    /// [FunctionAuthorization("110")]
    /// [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    /// int CreateUser(UserEntity user, out string errorCode);
    /// </code>
    /// </example>
    /// <seealso cref="InvokeMessageSink" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class FunctionAuthorizationAttribute : System.Attribute
    {
        public string FunctionCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAuthorizationAttribute" /> class.
        /// </summary>
        /// <param name="functionCode">The authorization Id</param>
        public FunctionAuthorizationAttribute(string functionCode)
        {
            this.FunctionCode = functionCode;
        }
    }
}