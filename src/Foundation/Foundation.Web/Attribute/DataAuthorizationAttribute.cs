
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;

namespace SE.DSP.Foundation.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DataAuthorizationAttribute : System.Attribute
    {
        public int AuthParaIndex { get; set; }
        public DataAuthType AuthType { get; set; }
        public string AuthItemIdName { get; set; }

        public DataAuthorizationAttribute(DataAuthType authType, int authParaIndex, string authItemIdName)
        {
            this.AuthParaIndex = authParaIndex;
            this.AuthType = authType;
            this.AuthItemIdName = authItemIdName;
        }

        public DataAuthorizationAttribute(DataAuthType authType, string authItemIdName)
        {
            this.AuthType = authType;
            this.AuthItemIdName = authItemIdName;
        }
    }
}