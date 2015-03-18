/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Net;
using System.Xml;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenTableService.Model;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    internal class OtsExceptionHandler : ResponseHandler
    {
        public OtsExceptionHandler()
        {
        }

        public override void Handle(ServiceResponse response)
        {
            base.Handle(response);
            
            if (response.IsSuccessful())
            {
                return;
            }

            ErrorResult errorResult = null;
            try
            {
                string contentType = response.Headers.ContainsKey(HttpHeaders.ContentType) ?
                                   response.Headers[HttpHeaders.ContentType] : response.Headers[ProtoBufConstant.HeaderContentType]; 
                
                IDeserializer<Stream, ErrorResult> d =
                    new DeserializerFactory().CreateDeserializer<ErrorResult>(contentType);

                if (d == null)
                    // Re-throw the web exception if the response cannot be parsed.
                    response.EnsureSuccessful();

                errorResult =  d.Deserialize(response.Content);
            }
            catch (ResponseDeserializationException)
            {
                // Re-throw the web exception if the response cannot be parsed.
                response.EnsureSuccessful();
            }

            // This throw must be out of the try block because otherwise
            // the exception would be caught be the following catch.
            throw ExceptionFactory.CreateException(errorResult, null);
        }
    }
}
