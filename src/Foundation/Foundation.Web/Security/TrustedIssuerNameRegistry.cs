
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.IdentityModel.Tokens;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// This class verifies that the issuer is trusted, and provides the issuer name.
    /// </summary>
    public class TrustedIssuerNameRegistry : System.IdentityModel.Tokens.IssuerNameRegistry
    {
        /// <summary>
        /// Gets the issuer name of the given security token,
        /// if it is the X509SecurityToken of 'localhost'.
        /// </summary>
        /// <param name="securityToken">The issuer's security token</param>
        /// <returns>A string that represents the issuer name</returns>
        /// <exception cref="SecurityTokenException">If the issuer is not trusted.</exception>
        public override string GetIssuerName(SecurityToken securityToken)
        {
            X509SecurityToken x509Token = securityToken as X509SecurityToken;
            if (x509Token != null)
            {
                var thumbprint = ConfigHelper.Get(STSConstant.CertificateThumbprint);

                if (!String.IsNullOrWhiteSpace(thumbprint) && !String.IsNullOrWhiteSpace(x509Token.Certificate.Thumbprint)
                    && String.Equals(x509Token.Certificate.Thumbprint.Replace(" ", "").ToLower(), thumbprint.Replace(" ","").ToLower()))
                {
                    return x509Token.Certificate.SubjectName.Name;
                }
            }

            throw new SecurityTokenException("Untrusted issuer.");
        }
    }
}
