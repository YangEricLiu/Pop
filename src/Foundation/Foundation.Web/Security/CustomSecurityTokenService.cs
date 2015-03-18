using System;
using System.Linq;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using SE.DSP.Foundation.Infrastructure.Utils;
using System.IdentityModel.Configuration;
using System.IdentityModel;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Claims;
using System.IdentityModel.Tokens;

namespace SE.DSP.Foundation.Web
{

    /// <summary>
    /// A custom SecurityTokenService implementation.
    /// </summary>
    public class CustomSecurityTokenService : SecurityTokenService
    {
        // TODO: Set enableAppliesToValidation to true to enable only the RP Url's specified in the PassiveRedirectBasedClaimsAwareWebApps array to get a token from this STS
        private static bool enableAppliesToValidation = false;

        // TODO: Add relying party Url's that will be allowed to get token from this STS
        private static readonly string[] PassiveRedirectBasedClaimsAwareWebApps =
            {
                /*"https://localhost/stshost"*/
            };

        /// <summary>
        /// Creates an instance of CustomSecurityTokenService.
        /// </summary>
        /// <param name="configuration">The SecurityTokenServiceConfiguration.</param>
        public CustomSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Validates appliesTo and throws an exception if the appliesTo is null or contains an unexpected address.
        /// </summary>
        /// <param name="appliesTo">The AppliesTo value that came in the RST.</param>
        /// <exception cref="ArgumentNullException">If 'appliesTo' parameter is null.</exception>
        /// <exception cref="InvalidRequestException">If 'appliesTo' is not valid.</exception>
        private void ValidateAppliesTo(EndpointReference appliesTo)
        {
            if (appliesTo == null)
            {
                throw new ArgumentNullException("appliesTo");
            }

            // TODO: Enable AppliesTo validation for allowed relying party Urls by setting enableAppliesToValidation to true. By default it is false.
            if (enableAppliesToValidation)
            {
                bool validAppliesTo = false;
                foreach (string rpUrl in PassiveRedirectBasedClaimsAwareWebApps)
                {
                    if (appliesTo.Uri.Equals(new Uri(rpUrl)))
                    {
                        validAppliesTo = true;
                        break;
                    }
                }

                if (!validAppliesTo)
                {
                    throw new InvalidRequestException(String.Format("The 'appliesTo' address '{0}' is not valid.",
                                                                    appliesTo.Uri.OriginalString));
                }
            }
        }

        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token for a
        /// single RP identity represented by the EncryptingCertificateName.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <returns>The scope information to be used for the token issuance.</returns>
        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            ValidateAppliesTo(request.AppliesTo);

            //
            // Note: The signing certificate used by default has a Distinguished name of "CN=STSTestCert",
            // and is located in the Personal certificate store of the Local Computer. Before going into production,
            // ensure that you change this certificate to a valid CA-issued certificate as appropriate.
            //
            Scope scope = new Scope(request.AppliesTo.Uri.OriginalString,
                                    SecurityTokenServiceConfiguration.SigningCredentials);

            string encryptingCertificateName = WebConfigurationManager.AppSettings["EncryptingCertificateName"];
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                // Important note on setting the encrypting credentials.
                // In a production deployment, you would need to select a certificate that is specific to the RP that is requesting the token.
                // You can examine the 'request' to obtain information to determine the certificate to use.
                scope.EncryptingCredentials =
                    new X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My,
                                                                                 StoreLocation.LocalMachine,
                                                                                 encryptingCertificateName));
            }
            else
            {
                // If there is no encryption certificate specified, the STS will not perform encryption.
                // This will succeed for tokens that are created without keys (BearerTokens) or asymmetric keys.  
                scope.TokenEncryptionRequired = false;
            }

            // Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
            // In this template, we have chosen to set this to the AppliesToAddress.
            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }


        /// <summary>
        /// This method returns the claims to be issued in the token.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST, can be used to obtain addtional information.</param>
        /// <param name="scope">The scope information corresponding to this request.</param> 
        /// <exception cref="ArgumentNullException">If 'principal' parameter is null.</exception>
        /// <returns>The outgoing claimsIdentity to be included in the issued token.</returns>
        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal,
                                                                   RequestSecurityToken request, Scope scope)
        {
            if (null == principal)
            {
                throw new ArgumentNullException("principal");
            }

            ClaimsIdentity outputIdentity = new ClaimsIdentity();

            // Issue custom claims.
            // TODO: Change the claims below to issue custom claims required by your application.
            // Update the application's configuration file too to reflect new claims requirement.

            var issuerName = System.Configuration.ConfigurationManager.AppSettings[STSConstant.IssuerName];

            User user = CookieUtil.GetCookie();

            outputIdentity.AddClaim(new Claim(System.IdentityModel.Claims.ClaimTypes.Name, principal.Identity.Name));
            outputIdentity.AddClaim(new Claim(STSConstant.IPSTSName, issuerName));
            outputIdentity.AddClaim(new Claim(STSConstant.RemUserId, user.Id.ToString(CultureInfo.InvariantCulture)));
            outputIdentity.AddClaim(new Claim(STSConstant.RemSpId, user.SPId.ToString(CultureInfo.InvariantCulture)));
            outputIdentity.AddClaim(new Claim(STSConstant.RemDemoStatus, user.DemoStatus.ToString(CultureInfo.InvariantCulture)));


            var qryStr = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["wctx"]);
            var qryArr = qryStr.Split('&');
            var rukvStr = qryArr.FirstOrDefault(p=>p.StartsWith("ru="));
            var ru = rukvStr.Substring(3);
            //var decodeRu= HttpUtility


            //outputIdentity.Claims.Add(new Claim(STSConstant.Language, ));
            //outputIdentity.Claims.Add(new Claim(STSConstant.Language, user.DemoStatus.ToString(CultureInfo.InvariantCulture)));
            //outputIdentity.Claims.Add(new Claim(STSConstant.RemUserRealName, user.RealName.ToString(CultureInfo.InvariantCulture)));
            //outputIdentity.Claims.Add(new Claim(STSConstant.RemUserType, user.UserType.ToString(CultureInfo.InvariantCulture)));
            //outputIdentity.Claims.Add(new Claim(STSConstant.RemUserVersion, user.Version.ToString()));

            return outputIdentity;
        }
    }
}
