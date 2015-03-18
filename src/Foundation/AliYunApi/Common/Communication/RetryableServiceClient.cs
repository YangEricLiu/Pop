/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// Represents the <see cref="IServiceClient"/> implementation
    /// that can automatically retry the request operations if they are failed
    /// due to specific exceptions.
    /// </summary>
    internal class RetryableServiceClient : IServiceClient
    {

        #region Fields & Properties
        private IServiceClient _innerClient;

        public Func<Exception, bool> ShouldRetryCallback { get; set; }

        /// <summary>
        /// Gets or sets the max retry times on error.
        /// </summary>
        public int MaxErrorRetry { get; set; }

        public static int RetryPauseScale { get; set; }

        #endregion

        #region Constructors

        public RetryableServiceClient(IServiceClient innerClient)
        {
            Debug.Assert(innerClient != null);
            _innerClient = innerClient;

            MaxErrorRetry = 3;
            RetryPauseScale = 300; // milliseconds.
        }

        #endregion

        #region IServiceClient Members

        public ServiceResponse Send(ServiceRequest request, ExecutionContext context)
        {
            return SendImpl(request, context, 0);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
        private ServiceResponse SendImpl(ServiceRequest request, ExecutionContext context, int retries)
        {
            try
            {
                return _innerClient.Send(request, context);
            }
            catch (Exception ex)
            {
                if (ShouldRetry(ex, retries))
                {
                    Pause(retries);
                    return SendImpl(request, context, ++retries);
                }
                else
                {
                    throw ex;
                }
            }
        }

        public IAsyncResult BeginSend(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state)
        {
            var asyncResult = new RetryableAsyncResult(callback, state, request, context);

            BeginSendImpl(request, context, asyncResult);

            return asyncResult;
        }

        private void BeginSendImpl(ServiceRequest request, ExecutionContext context, RetryableAsyncResult asyncResult)
        {
            if (asyncResult.InnerAsyncResult != null)
            {
                asyncResult.InnerAsyncResult.Dispose();
            }

            asyncResult.InnerAsyncResult =
                _innerClient.BeginSend(request, context, OnBeginSendCompleted, asyncResult) as AsyncResult;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Catch the exception to dispatch it another thread with async result.")]
        private void OnBeginSendCompleted(IAsyncResult ar)
        {
            var retryableAsyncResult = ar.AsyncState as RetryableAsyncResult;

            try
            {
                // Success.
                var result = _innerClient.EndSend(ar);
                retryableAsyncResult.Complete(result);
            }
            catch (Exception ex)
            {
                if (ShouldRetry(ex, retryableAsyncResult.Retries))
                {
                    // Retry
                    // Don't forget to increment the retries.
                    Pause(retryableAsyncResult.Retries++);
                    BeginSendImpl(retryableAsyncResult.Request,
                                  retryableAsyncResult.Context,
                                  retryableAsyncResult);
                }
                else
                {
                    retryableAsyncResult.Complete(ex);
                }
            }
        }

        public ServiceResponse EndSend(IAsyncResult ar)
        {
            var retryableAsyncResult = ar as RetryableAsyncResult;
            Debug.Assert(ar != null);

            try
            {
                var result = retryableAsyncResult.GetResult();
                retryableAsyncResult.Dispose();

                return result;
            }
            catch (ObjectDisposedException)
            {
                throw new InvalidOperationException(Properties.Resources.ExceptionEndOperationHasBeenCalled);
            }
        }

        private bool ShouldRetry(Exception ex, int retries)
        {
            if (retries > this.MaxErrorRetry)
            {
                return false;
            }

            WebException webException = ex as WebException;
            if (webException != null)
            {
                HttpWebResponse httpWebResponse = webException.Response as HttpWebResponse;
                if (httpWebResponse != null &&
                    (httpWebResponse.StatusCode == HttpStatusCode.ServiceUnavailable ||
                     httpWebResponse.StatusCode == HttpStatusCode.InternalServerError))
                {
                    return true;
                }
            }

            if (ShouldRetryCallback != null && ShouldRetryCallback(ex))
            {
                return true;
            }

            return false;
        }

        private static void Pause(int retries)
        {
            // make the pause time increase exponentially
            // based on an assumption that the more times it retries,
            // the less probability it succeeds.
            int scale = RetryPauseScale;
            int delay = (int)Math.Pow(2, retries) * scale;

            Thread.Sleep(delay);
        }

        #endregion

    }

    internal class RetryableAsyncResult : AsyncResult<ServiceResponse>
    {
        public ServiceRequest Request { get; private set; }

        public ExecutionContext Context { get; private set; }

        public AsyncResult InnerAsyncResult { get; set; }

        public int Retries { get; set; }

        public RetryableAsyncResult(AsyncCallback callback, object state,
                                    ServiceRequest request, ExecutionContext context)
            : base(callback, state)
        {
            Debug.Assert(request != null);
            this.Request = request;
            this.Context = context;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && InnerAsyncResult != null)
            {
                this.InnerAsyncResult.Dispose();
                this.InnerAsyncResult = null;
            }
        }
    }
}


