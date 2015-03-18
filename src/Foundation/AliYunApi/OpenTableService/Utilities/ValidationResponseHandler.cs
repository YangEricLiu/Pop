/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// Description of ValidationResponseHandler.
    /// </summary>
    internal class ValidationResponseHandler : ResponseHandler
    {
        private const int _responseTimeoutMinutes = 15; // unit: minutes.

        private ServiceCredentials _credentials;
        private string _action; // The OTS action

        public ValidationResponseHandler(ServiceCredentials credentials, string action)
        {
            Debug.Assert(!string.IsNullOrEmpty(action) && credentials != null);
            _credentials = credentials;
            _action = action;
        }
        
        public override void Handle(ServiceResponse response)
        {
            base.Handle(response);

            const string otsDateKey = "x-ots-date";
            // Because OTS uses "Content-Md5" rather than "Content-MD5" which is more common
            // for this header key, we don't put in the util class HttpHeaders.
            const string contentMd5Key = "Content-Md5";

            var headers = response.Headers;

            if (!headers.Keys.Contains(otsDateKey))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, "x-ots-date"),
                                                                      null);

            if (!headers.Keys.Contains(contentMd5Key))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, "Content-Md5"),
                                                                      null);

            if (!headers.Keys.Contains(HttpHeaders.ContentType))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, "Content-Type"),
                                                                      null);
            if (!headers.Keys.Contains(HttpHeaders.Authorization))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      OtsExceptions.ResponseFailedAuthorization,
                                                                      null);
            
            // TODO: Verify Content-MD5

            // Verification date
            var otsDate = headers["x-ots-date"];
            if (string.IsNullOrEmpty(otsDate))
            {
                throw ExceptionFactory.CreateInvalidResponseException(
                    response, OtsExceptions.ResponseExpired, null);
            }
            DateTime responseTime = DateUtils.ParseRfc822Date(otsDate);
            var timeSpan = DateTime.UtcNow.Subtract(responseTime);
            if (timeSpan.TotalMinutes > _responseTimeoutMinutes)
            {
                throw ExceptionFactory.CreateInvalidResponseException(
                    response, OtsExceptions.ResponseExpired, null);
            }

            // Validate authorization.
            StringBuilder canonicalizedOtsHeader = new StringBuilder();
            foreach (var key in headers.Keys)
            {
                if (key.StartsWith("x-ots-", StringComparison.OrdinalIgnoreCase))
                {
                    canonicalizedOtsHeader.AppendFormat(CultureInfo.InvariantCulture,
                                                        "{0}:{1}\n", key, headers[key]);
                }
            }

            var contentMd5 = headers[contentMd5Key];
            var contentType = headers[HttpHeaders.ContentType];
            var canonicalizedResource = "/" + _action;

            var data =
                contentMd5 + "\n" +
                contentType + "\n" +
                canonicalizedOtsHeader.ToString() +
                canonicalizedResource;

            string actual = ServiceSignature.Create().ComputeSignature(_credentials.AccessKey, data);
            string authorizationHeader = headers[HttpHeaders.Authorization];
            bool authEqual = false;
            if(authorizationHeader.Contains(':')){
                string expected = authorizationHeader.Split(':').Last();
                authEqual = expected.EndsWith(actual, StringComparison.Ordinal);
            }
            if (!authEqual){
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      OtsExceptions.ResponseFailedAuthorization, null);
            }
        }
    }
}
