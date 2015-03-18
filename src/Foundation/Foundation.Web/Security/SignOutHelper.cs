using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System.IdentityModel.Services;


namespace SE.DSP.Foundation.Web
{
    public static class StsHelper
    {
        public static void SignOut(Language language)
        {            
            //var fedAuthModule = FederatedAuthentication.WSFederationAuthenticationModule;
            //string signoutUrl = WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(fedAuthModule.Issuer, fedAuthModule.Realm, null);

            //var sessionAuthModule = FederatedAuthentication.SessionAuthenticationModule;
            //if (sessionAuthModule != null)
            //{
            //    sessionAuthModule.SignOut();
            //    sessionAuthModule.DeleteSessionTokenCookie();
            //}

            //WSFederationAuthenticationModule fam = (WSFederationAuthenticationModule)System.Web.HttpContext.Current.ApplicationInstance.Modules["WSFederationAuthenticationModule"];
            //if (fam != null) fam.SignOut(false);

            //FederatedAuthentication.SessionAuthenticationModule.SignOut();
            //FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
            //FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);
            //FormsAuthentication.SignOut();
            //fedAuthModule.SignOut(true);

            //return signoutUrl;

            WSFederationAuthenticationModule authModule = FederatedAuthentication.WSFederationAuthenticationModule;
            var replyUrl = authModule.Realm + "index.aspx?lang=" + I18nHelper.LocaleEnumToString(language);
            replyUrl.Replace("//", "/");
            WSFederationAuthenticationModule.FederatedSignOut(new Uri(authModule.Issuer), new Uri(replyUrl));
        }
    }
}
