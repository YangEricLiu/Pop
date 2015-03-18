using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SE.DSP.Foundation.Web
{
    /// <summary>
    /// A utility class which helps to retrieve an x509 certificate
    /// </summary>
    public class CertificateUtil
    {
        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, string thumbprint)
        {
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                X509Certificate2 result = null;

                //
                // Every time we call store.Certificates property, a new collection will be returned.
                //
                certificates = store.Certificates;

                for (int i = 0; i < certificates.Count; i++)
                {
                    X509Certificate2 cert = certificates[i];

                    if (!String.IsNullOrWhiteSpace(cert.Thumbprint))
                    {
                        var certThumbprint = cert.Thumbprint.Replace(" ", "").ToLower();
                        var validThumbprint = thumbprint.Replace(" ", "").ToLower();
                        if (certThumbprint != validThumbprint) continue;
                        
                        if (result != null)
                        {
                            throw new ApplicationException(
                                string.Format("There are multiple certificates for subject Name {0}", thumbprint));
                        }

                        result = new X509Certificate2(cert);
                    }
                }

                if (result == null)
                {
                    throw new ApplicationException(string.Format("No certificate was found for subject Name {0}",
                                                                 thumbprint));
                }

                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    for (int i = 0; i < certificates.Count; i++)
                    {
                        X509Certificate2 cert = certificates[i];
                        cert.Reset();
                    }
                }

                store.Close();
            }
        }
    }
}
