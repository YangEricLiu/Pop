using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Util;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// This SampleRequestValidator validates the wresult parameter of the
    /// WS-Federation passive protocol by checking for a SignInResponse message
    /// in the form post. The SignInResponse message contents are verified later by
    /// the WSFederationPassiveAuthenticationModule or the WIF signin controls.
    /// </summary>

    public class SampleRequestValidator : RequestValidator
    {
        protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            validationFailureIndex = 0;
            if (requestValidationSource == RequestValidationSource.Form &&
                collectionKey.Equals("wresult", StringComparison.Ordinal))
            {
                if (WSFederationMessage.CreateFromNameValueCollection(WSFederationMessage.GetBaseUrl(context.Request.Url), context.Request.Unvalidated.Form) as SignInResponseMessage != null)
                {
                    return true;
                }
            }
            return base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
        }
    }
}