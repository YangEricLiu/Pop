/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Base class for OTS commands.
    /// </summary>
    internal abstract class OtsCommand
    {
        public const string UrlEncodedContentType =
            "application/x-www-form-urlencoded; charset=utf-8";

        public ExecutionContext Context { get; private set; }

        public IServiceClient Client { get; private set; }

        public Uri Endpoint { get; private set; }

        protected abstract HttpMethod Method { get; }

        protected abstract String ResourcePath { get; }

        protected OtsCommand(IServiceClient client, Uri endpoint, ExecutionContext context)
        {
            Debug.Assert(client != null && endpoint != null && context != null);
            Endpoint = endpoint;
            Client = client;
            Context = context;
        }

        public void Execute()
        {
            // The response should be disposed.
            using (Client.Send(CreateRequest(), Context))
            {
            }
        }

        public IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            return Client.BeginSend(CreateRequest(),
                                    Context,
                                    callback, state);
        }

        public static void EndExecute(IServiceClient client, IAsyncResult asyncResult)
        {
            Debug.Assert(client != null);
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            // Should dispose resources.
            using (client.EndSend(asyncResult))
            {
            }
        }

        protected ServiceRequest CreateRequest()
        {
            var request = new ServiceRequest();
            request.Method = this.Method;
            request.Endpoint = this.Endpoint;
            request.ResourcePath = this.ResourcePath;
            request.Headers[HttpHeaders.ContentType] = UrlEncodedContentType;
            AddRequestParameters(request.Parameters);

            return request;
        }

        protected virtual void AddRequestParameters(IDictionary<string, string> parameters)
        {
        }

        /// <summary>
        /// add re
        /// </summary>
        /// <param name="headers"></param>
        protected virtual void AddRequestHeaders(IDictionary<string, string> headers)
        {
        }
    }

    /// <summary>
    /// Base class for OTS commands that return response objects.
    /// </summary>
    internal abstract class OtsCommand<T> : OtsCommand
    {

        protected OtsCommand(IServiceClient client, Uri endpoint, ExecutionContext context)
            : base(client, endpoint, context)
        {
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns></returns>
        public new T Execute()
        {
            using (var response = Client.Send(CreateRequest(), Context))
            {
                return GetResultFromResponse(response);
            }
        }

        public new static T EndExecute(IServiceClient client, IAsyncResult asyncResult)
        {
            Debug.Assert(client != null);
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            using (var response = client.EndSend(asyncResult))
            {
                return GetResultFromResponse(response);
            }
        }

        private static T GetResultFromResponse(ServiceResponse response)
        {
            try
            {
                string contentType = response.Headers.ContainsKey(HttpHeaders.ContentType) ?
                    response.Headers[HttpHeaders.ContentType] : response.Headers[ProtoBufConstant.HeaderContentType];

                IDeserializer<Stream, T> d = new DeserializerFactory().CreateDeserializer<T>(contentType);

                if (d == null)
                    throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                          OtsExceptions.ResponseDataIncomplete,
                                                                          null);

                return d.Deserialize(response.Content);
            }
            catch (ResponseDeserializationException ex)
            {
                throw ExceptionFactory.CreateInvalidResponseException(response,
                                                                      OtsExceptions.ResponseDataIncomplete,
                                                                      ex);
            }
        }
    }
}
