﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// Represents the information for sending requests.
    /// </summary>
    internal class ServiceRequest : ServiceMessage
    {
        private IDictionary<String, String> parameters =
            new Dictionary<String, String>();

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the resource path of the request URI.
        /// </summary>
        public String ResourcePath { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets the dictionary of the request parameters.
        /// </summary>
        public IDictionary<String, String> Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public ServiceRequest()
        {
        }

        /// <summary>
        /// Build the request URI from the request message.
        /// </summary>
        /// <returns></returns>
        public string BuildRequestUri()
        {
            const string delimiter = "/";
            String uri = Endpoint.ToString();
            if (!uri.EndsWith(delimiter)
                && (ResourcePath == null ||
                    !ResourcePath.StartsWith(delimiter)))
            {
                uri += delimiter;
            }

            if (ResourcePath != null)
            {
                uri += ResourcePath;
            }

            if (IsParameterInUri())
            {
                String paramString = HttpUtils.GetRequestParameterString(parameters, OtsConstants.ApiVersion);
                if (!string.IsNullOrEmpty(paramString))
                {
                    uri += "?" + paramString;
                }
            }

            return uri;
        }

        public Stream BuildRequestContent()
        {
            if (!IsParameterInUri())
            {
                if (this.Parameters.Keys.Contains(ProtoBufConstant.ParameterContent))
                {
                    byte[] buffer = parameters[ProtoBufConstant.ParameterContent].Split(ProtoBufConstant.Separator).Select(item => Convert.ToByte(item)).ToArray();
                    Stream content = new MemoryStream();
                    content.Write(buffer, 0, buffer.Length);
                    content.Flush();
                    // Move the marker to the beginning for further read.
                    content.Seek(0, SeekOrigin.Begin);
                    return content;
                }
                else
                {
                    String paramString = HttpUtils.GetRequestParameterString(parameters, OtsConstants.ApiVersion);
                    if (!string.IsNullOrEmpty(paramString))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(paramString);
                        Stream content = new MemoryStream();
                        content.Write(buffer, 0, buffer.Length);
                        content.Flush();
                        // Move the marker to the beginning for further read.
                        content.Seek(0, SeekOrigin.Begin);
                        return content;
                    }
                }
            }

            return this.Content;
        }

        private bool IsParameterInUri()
        {
            bool requestHasNoPayload = this.Content != null;
            bool requestIsPost = this.Method == HttpMethod.Post;
            bool putParamsInUri = !requestIsPost || requestHasNoPayload;
            return putParamsInUri;
        }
    }
}
