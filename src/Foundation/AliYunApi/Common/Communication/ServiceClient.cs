﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

using Aliyun.OpenServices.Common;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Utilities;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// The default implementation of <see cref="IServiceClient" />.
    /// </summary>
    internal abstract class ServiceClient : IServiceClient
    {

        #region Fields and Properties

        private Aliyun.OpenServices.Ots.ClientConfiguration _configuration;
        
        protected Aliyun.OpenServices.Ots.ClientConfiguration Configuration
        {
            get { return _configuration; }
        }

        #endregion

        #region Constructors

        protected ServiceClient(Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            Debug.Assert(configuration != null);
            // Make a definsive copy to ensure the class is immutable.
            _configuration = (Aliyun.OpenServices.Ots.ClientConfiguration)configuration.Clone();
        }
        
        public static ServiceClient Create(Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            return new ServiceClientImpl(configuration);
        }

        #endregion
        
        #region IServiceClient Members

        public ServiceResponse Send(ServiceRequest request, ExecutionContext context)
        {
            Debug.Assert(request != null);

            SignRequest(request, context);
            ServiceResponse response = SendCore(request, context);
            HandleResponse(response, context.ResponseHandlers);
            return response;
        }

        public IAsyncResult BeginSend(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state)
        {
            Debug.Assert(request != null);

            SignRequest(request, context);
            return BeginSendCore(request, context, callback, state);
        }

        public ServiceResponse EndSend(IAsyncResult aysncResult)
        {
            var ar = aysncResult as AsyncResult<ServiceResponse>;
            Debug.Assert(ar != null);

            try
            {
                // Must dispose the async result instance.
                var result = ar.GetResult();
                ar.Dispose();

                return result;
            }
            catch (ObjectDisposedException)
            {
                throw new InvalidOperationException(Properties.Resources.ExceptionEndOperationHasBeenCalled);
            }
        }

        #endregion

        protected abstract ServiceResponse SendCore(ServiceRequest request, ExecutionContext context);
        
        protected abstract IAsyncResult BeginSendCore(ServiceRequest request, ExecutionContext context, AsyncCallback callback, Object state);
        
        private static void SignRequest(ServiceRequest request, ExecutionContext context)
        {
            if (context.Signer != null)
            {
                context.Signer.Sign(request, context.Credentials);
            }
        }
        
        protected static void HandleResponse(ServiceResponse response, IList<IResponseHandler> handlers)
        {
            foreach(IResponseHandler handler in handlers)
            {
                handler.Handle(response);
            }
        }
    }
}
