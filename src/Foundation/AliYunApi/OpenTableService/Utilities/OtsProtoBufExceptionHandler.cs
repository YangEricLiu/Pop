/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.ProtoBuf;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    internal class OtsProtoBufExceptionHandler : ResponseHandler
    {
        public OtsProtoBufExceptionHandler()
        {
        }

        public override void Handle(ServiceResponse response)
        {
            base.Handle(response);

            if (response.IsSuccessful())
            {
                return;
            }

            ErrorMessage errorResult = null;
            try
            {
                string contentType = response.Headers.ContainsKey(HttpHeaders.ContentType) ?
                                   response.Headers[HttpHeaders.ContentType] : response.Headers[ProtoBufConstant.HeaderContentType];

                IDeserializer<Stream, ErrorMessage> d =
                    new DeserializerFactory().CreateDeserializer<ErrorMessage>(contentType);

                if (d == null)
                    // Re-throw the web exception if the response cannot be parsed.
                    response.EnsureSuccessful();

                errorResult = d.Deserialize(response.Content);
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
