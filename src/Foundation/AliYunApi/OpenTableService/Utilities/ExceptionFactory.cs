/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.ProtoBuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// The factory to create an instance of <see cref="OtsException"/>.
    /// </summary>
    internal static class ExceptionFactory
    {
        public static OtsException CreateException(string errorCode,
                                                   string message,
                                                   string requestId,
                                                   string hostId)
        {
            return CreateException(errorCode, message, requestId, hostId, null);
        }

        public static OtsException CreateException(string errorCode,
                                                   string message,
                                                   string requestId,
                                                   string hostId,
                                                   Exception innerException)
        {
            OtsException exception = innerException != null ?
                new OtsException(message, innerException) :
                new OtsException(message);

            exception.ErrorCode = errorCode;
            exception.RequestId = requestId;
            exception.HostId = hostId;

            return exception;
        }

        public static OtsException CreateException(ErrorResult errorResult,
                                                   Exception innerException)
        {
            Debug.Assert(errorResult != null);
            return CreateException(errorResult.Code, errorResult.Message,
                                   errorResult.RequestId, errorResult.HostId, innerException);
        }

        public static OtsException CreateException(ErrorMessage errorMessage,
                                           Exception innerException)
        {
            Debug.Assert(errorMessage != null);
            return CreateException(errorMessage.code, errorMessage.message,
                                   string.Empty, string.Empty, innerException);
        }


        public static Exception CreateInvalidResponseException(ServiceResponse response,
                                                               string message,
                                                               Exception innerException)
        {
            string requestId = null;
            string hostId = null;

            if (response != null)
            {
                try
                {
                    var responseString = ReadResponseAsString(response);

                    var requestIdRegex = new Regex(@"\<RequestID\>(\w+)\</RequestID\>");
                    var requestIdMatch = requestIdRegex.Match(responseString);
                    if (requestIdMatch.Success)
                    {
                        requestId = requestIdMatch.Groups[1].Value;
                    }
                    var hostIdRegex = new Regex(@"\<HostID\>(\w+)\</HostID\>");
                    var hostIdMatch = hostIdRegex.Match(responseString);
                    if (hostIdMatch.Success)
                    {
                        hostId = hostIdMatch.Groups[1].Value;
                    }
                }
                catch (InvalidOperationException)
                {
                    // Intentially not throw.
                }
            }
            if (!string.IsNullOrEmpty(requestId) || !string.IsNullOrEmpty(hostId))
            {
                message += string.Format(CultureInfo.InvariantCulture,
                                         OtsExceptions.InvalidResponseMessage, requestId, hostId);
            }

            return new InvalidOperationException(message, innerException);
        }

        private static string ReadResponseAsString(ServiceResponse response)
        {
            using (var responseStream = response.Content)
            {
                const int bufferSize = 4 * 1024;

                StringBuilder stringBuilder = new StringBuilder();

                var buffer = new byte[bufferSize];
                while (responseStream.Read(buffer, 0, buffer.Length) > 0)
                {
                    var bufString = OtsUtility.DataEncoding.GetString(buffer);
                    stringBuilder.Append(bufString);
                }

                return stringBuilder.ToString();
            }
        }
    }
}
