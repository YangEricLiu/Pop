/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// Description of ValidationResponseHandler.
    /// </summary>
    internal class ValidationProtoBufResponseHandler : ResponseHandler
    {
        private const int _responseTimeoutMinutes = 15; // unit: minutes.

        private ServiceCredentials _credentials;
        private string _action; // The OTS action

        public ValidationProtoBufResponseHandler(ServiceCredentials credentials, string action)
        {
            Debug.Assert(!string.IsNullOrEmpty(action) && credentials != null);
            _credentials = credentials;
            _action = action;
        }

        public override void Handle(ServiceResponse response)
        {
            base.Handle(response);

            var headers = response.Headers;

            if (!headers.Keys.Contains(ProtoBufConstant.HeaderDate))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, ProtoBufConstant.HeaderDate),
                                                                      null);

            if (!headers.Keys.Contains(ProtoBufConstant.HeaderContentMd5))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, ProtoBufConstant.HeaderContentMd5),
                                                                      null);

            if (!headers.Keys.Contains(ProtoBufConstant.HeaderContentType))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, ProtoBufConstant.HeaderContentType),
                                                                      null);
            if (!headers.Keys.Contains(ProtoBufConstant.HeaderAuthorization))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      OtsExceptions.ResponseFailedAuthorization,
                                                                      null);

            if (!headers.Keys.Contains(ProtoBufConstant.HeaderRequestId))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, ProtoBufConstant.HeaderRequestId),
                                                                      null);

            if (!headers.Keys.Contains(ProtoBufConstant.HeaderHostId))
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      string.Format(CultureInfo.CurrentUICulture, OtsExceptions.ResponseDoesNotContainHeader, ProtoBufConstant.HeaderHostId),
                                                                      null);

            // Verification date
            var otsDate = headers[ProtoBufConstant.HeaderDate];
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

            //// Validate content md5
            //StringBuilder canonicalizedOtsHeader = new StringBuilder();
            //foreach (var key in headers.Keys)
            //{
            //    if (key.StartsWith("x-ots-", StringComparison.OrdinalIgnoreCase))
            //    {
            //        canonicalizedOtsHeader.AppendFormat(CultureInfo.InvariantCulture,
            //                                            "{0}:{1}\n", key, headers[key]);
            //    }
            //}

            //var contentMd5 = headers[contentMd5Key];
            //var contentType = headers[HttpHeaders.ContentType];
            //var canonicalizedResource = "/" + _action;

            //var data =
            //    contentMd5 + "\n" +
            //    contentType + "\n" +
            //    canonicalizedOtsHeader.ToString() +
            //    canonicalizedResource;

            //string actual = ServiceSignature.Create().ComputeSignature(_credentials.AccessKey, data);
            //string authorizationHeader = headers[HttpHeaders.Authorization];
            //bool authEqual = false;
            //if (authorizationHeader.Contains(':'))
            //{
            //    string expected = authorizationHeader.Split(':').Last();
            //    authEqual = expected.EndsWith(actual, StringComparison.Ordinal);
            //}
            //if (!authEqual)
            //{
            //    throw ExceptionFactory.CreateInvalidResponseException(response,
            //                                                          OtsExceptions.ResponseFailedAuthorization, null);
            //}
        }
    }
}
