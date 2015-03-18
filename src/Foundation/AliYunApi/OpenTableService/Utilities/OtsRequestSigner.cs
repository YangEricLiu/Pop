/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// Description of OTSRequestSigner.
    /// </summary>
    internal class OtsRequestSigner : IRequestSigner
    {
        public OtsRequestSigner()
        {
        }

        public void Sign(ServiceRequest request, ServiceCredentials credentials)
        {
            AddRequiredParameters(request.Parameters,
                                  request.ResourcePath,
                                  credentials,
                                  ServiceSignature.Create(),
                                  DateTime.UtcNow);
        }

        private static void AddRequiredParameters(IDictionary<string, string> parameters,
                                                  string action,
                                                  ServiceCredentials credentials,
                                                  ServiceSignature signer,
                                                  DateTime timestamp)
        {
            const string signatureKey = "Signature";
            // Have to remove the previous Signature
            if (parameters.ContainsKey(signatureKey))
            {
                parameters.Remove(signatureKey);
            }
            parameters["Date"] = DateUtils.FormatRfc822Date(timestamp);
            parameters["OTSAccessKeyId"] = credentials.AccessId;
            parameters["APIVersion"] = OtsConstants.ApiVersion;
            parameters["SignatureMethod"] = signer.SignatureMethod;
            parameters["SignatureVersion"] = signer.SignatureVersion;

            var signature = GetSignature(credentials, signer, action, parameters);
            parameters[signatureKey] = signature;
        }

        private static string GetSignature(ServiceCredentials credentials,
                                           ServiceSignature signer,
                                           string action,
                                           IDictionary<string, string> parameters)
        {
            // The parameters to compute signature need to be sorted.
            var signatureData =
                string.Format(CultureInfo.InvariantCulture, "/{0}\n{1}", action,
                              HttpUtils.GetRequestParameterString(parameters.OrderBy(e => e.Key, StringComparer.Ordinal), OtsConstants.ApiVersion));

            var signature = signer.ComputeSignature(credentials.AccessKey, signatureData);
            return signature;
        }
    }
}
