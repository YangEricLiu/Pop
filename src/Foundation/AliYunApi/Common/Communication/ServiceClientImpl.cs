/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// An default <see cref="ServiceClient"/> implementation that
    /// communicates with Aliyun Services over the HTTP protocol.
    /// </summary>
    internal class ServiceClientImpl : ServiceClient
    {

        #region Embeded Classes

        /// <summary>
        /// Represents the async operation of requests in <see cref="ServiceClientImpl"/>.
        /// </summary>
        private class HttpAsyncResult : AsyncResult<ServiceResponse>
        {
            public HttpWebRequest WebRequest { get; set; }

            public ExecutionContext Context { get; set; }

            public HttpAsyncResult(AsyncCallback callback, object state)
                : base(callback, state)
            {
            }
        }

        /// <summary>
        /// Represents the response data of <see cref="ServiceClientImpl"/> requests.
        /// </summary>
        private class ResponseImpl : ServiceResponse
        {
            private bool _disposed;
            private HttpWebResponse _response;
            private Exception _failure;
            private IDictionary<string, string> _headers;

            public override HttpStatusCode StatusCode
            {
                get
                {
                    return _response.StatusCode;
                }
            }

            public override Exception Failure
            {
                get
                {
                    return this._failure;
                }
            }

            public override IDictionary<string, string> Headers
            {
                get
                {
                    ThrowIfObjectDisposed();
                    if (_headers == null)
                    {
                        _headers = GetResponseHeaders(_response);
                    }

                    return _headers;
                }
            }

            public override Stream Content
            {
                get
                {
                    ThrowIfObjectDisposed();

                    try
                    {
                        return _response != null ?
                            _response.GetResponseStream() : null;
                    }
                    catch (ProtocolViolationException ex)
                    {
                        throw new InvalidOperationException(ex.Message, ex);
                    }
                }
            }

            public ResponseImpl(HttpWebResponse httpWebResponse)
            {
                Debug.Assert(httpWebResponse != null);
                _response = httpWebResponse;

                Debug.Assert(this.IsSuccessful(), "This constructor only allows a successfull response.");
            }

            public ResponseImpl(WebException failure)
            {
                Debug.Assert(failure != null);
                HttpWebResponse httpWebResponse = failure.Response as HttpWebResponse;
                Debug.Assert(httpWebResponse != null);

                _failure = failure;
                _response = httpWebResponse;
                Debug.Assert(!this.IsSuccessful(), "This constructor only allows a failed response.");
            }

            private static IDictionary<string, string> GetResponseHeaders(HttpWebResponse response)
            {
                var headers = response.Headers;
                var result = new Dictionary<string, string>(headers.Count);

                for (int i = 0; i < headers.Count; i++)
                {
                    var key = headers.Keys[i];
                    var value = headers.Get(key);
                    result.Add(key,
                               HttpUtils.ReEncode(
                                   value,
                                   HttpUtils.Iso88591Charset,
                                   HttpUtils.Charset));
                }

                return result;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (_disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (_response != null)
                    {
                        _response.Close();
                        _response = null;
                    }
                    _disposed = true;
                }
            }

            private void ThrowIfObjectDisposed()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
            }
        }

        #endregion

        #region Constructors

        public ServiceClientImpl(Aliyun.OpenServices.Ots.ClientConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Implementations

        protected override ServiceResponse SendCore(ServiceRequest serviceRequest,
                                                    ExecutionContext context)
        {
            HttpWebRequest request = HttpFactory.CreateWebRequest(serviceRequest, Configuration);
            SetRequestContent(request, serviceRequest, false, null);

            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                return new ResponseImpl(response);
            }
            catch (WebException ex)
            {
                return HandlException(ex);
            }
        }

        protected override IAsyncResult BeginSendCore(ServiceRequest serviceRequest,
                                                      ExecutionContext context,
                                                      AsyncCallback callback, object state)
        {
            var request = HttpFactory.CreateWebRequest(serviceRequest, Configuration);

            var asyncResult = new HttpAsyncResult(callback, state);
            asyncResult.WebRequest = request;
            asyncResult.Context = context;

            SetRequestContent(request, serviceRequest, true,
                              () =>
                              {
                                  // Begin get response after the request is processed.
                                  request.BeginGetResponse(OnGetResponseCompleted, asyncResult);
                              });

            return asyncResult;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
                                                         Justification = "Catch the exception and set it to async result.")]
        private void OnGetResponseCompleted(IAsyncResult ar)
        {
            var asyncResult = ar.AsyncState as HttpAsyncResult;
            Debug.Assert(asyncResult != null && asyncResult.WebRequest != null);

            try
            {
                var response = asyncResult.WebRequest.EndGetResponse(ar) as HttpWebResponse;
                ServiceResponse res = new ResponseImpl(response);
                HandleResponse(res, asyncResult.Context.ResponseHandlers);
                asyncResult.Complete(res);
            }
            catch (WebException ex)
            {
                try
                {
                    ServiceResponse res = HandlException(ex);
                    HandleResponse(res, asyncResult.Context.ResponseHandlers);
                    asyncResult.WebRequest.Abort();
                    asyncResult.Complete(res);
                }
                catch (Exception ex2)
                {
                    asyncResult.WebRequest.Abort();
                    asyncResult.Complete(ex2);
                }
            }
            catch (Exception ex3)
            {
                asyncResult.WebRequest.Abort();
                asyncResult.Complete(ex3);
            }
        }

        private static void SetRequestContent(HttpWebRequest webRequest,
                                              ServiceRequest serviceRequest,
                                              bool async, Action asyncCallback)
        {
            var data = serviceRequest.BuildRequestContent();

            if (data == null ||
                (serviceRequest.Method != HttpMethod.Put &&
                 serviceRequest.Method != HttpMethod.Post))
            {
                // Skip setting content body in this case.
                if (async)
                {
                    asyncCallback();
                }
                return;
            }

            // Write data to the request stream.
            webRequest.ContentLength = data.Length;

            if (async)
            {
                Debug.Assert(asyncCallback != null);
                webRequest.BeginGetRequestStream(
                    (ar) =>
                    {
                        // Must call EndGetRequestStream to avoid resource leak.
                        using (var requestStream = webRequest.EndGetRequestStream(ar))
                        using (data)
                        {
                            data.WriteTo(requestStream);
                        }
                        asyncCallback();
                    }, null);
            }
            else
            {
                using (var requestStream = webRequest.GetRequestStream())
                using (data)
                {
                    data.WriteTo(requestStream);
                }
            }
        }

        private static ServiceResponse HandlException(WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response == null)
            {
                throw ex;
            }
            else
            {
                return new ResponseImpl(ex);
            }
        }

        #endregion

    }

    internal static class HttpFactory
    {
        internal static HttpWebRequest CreateWebRequest(ServiceRequest serviceRequest, Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            Debug.Assert(serviceRequest != null && configuration != null);

            HttpWebRequest webRequest = WebRequest.Create(serviceRequest.BuildRequestUri()) as HttpWebRequest;

            SetRequestHeaders(webRequest, serviceRequest, configuration);
            SetRequestProxy(webRequest, configuration);

            return webRequest;
        }

        // Set request headers
        private static void SetRequestHeaders(HttpWebRequest webRequest, ServiceRequest serviceRequest,
                                              Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            webRequest.Timeout = configuration.ConnectionTimeout;
            webRequest.Method = serviceRequest.Method.ToString().ToUpperInvariant();

            // Because it is not allowed to set common headers
            // with the WebHeaderCollection.Add method,
            // we have to call an internal method to skip validation.
            foreach (var h in serviceRequest.Headers)
            {
                webRequest.Headers.AddInternal(h.Key,
                                               HttpUtils.ReEncode(
                                                   h.Value,
                                                   HttpUtils.Charset,
                                                   HttpUtils.Iso88591Charset));
            }

            //for protobuf, the "x-ots-" parameters need add to headers
            foreach (var h in serviceRequest.Parameters)
            {
                if (h.Key.Contains("x-ots-"))
                {
                    webRequest.Headers.AddInternal(h.Key,
                                                  HttpUtils.ReEncode(
                                                      h.Value,
                                                      HttpUtils.Charset,
                                                      HttpUtils.Iso88591Charset));
                }
            }

            // Set user-agent
            if (!string.IsNullOrEmpty(configuration.UserAgent))
            {
                webRequest.UserAgent = configuration.UserAgent;
            }
        }

        // Set proxy
        private static void SetRequestProxy(HttpWebRequest webRequest, Aliyun.OpenServices.Ots.ClientConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.ProxyHost))
            {
                if (configuration.ProxyPort < 0)
                {
                    webRequest.Proxy = new WebProxy(configuration.ProxyHost);
                }
                else
                {
                    webRequest.Proxy = new WebProxy(configuration.ProxyHost, configuration.ProxyPort);
                }

                if (!string.IsNullOrEmpty(configuration.ProxyUserName))
                {
                    webRequest.Proxy.Credentials = String.IsNullOrEmpty(configuration.ProxyDomain) ?
                        new NetworkCredential(configuration.ProxyUserName, configuration.ProxyPassword ?? string.Empty) :
                        new NetworkCredential(configuration.ProxyUserName, configuration.ProxyPassword ?? string.Empty,
                                              configuration.ProxyDomain);
                }
            }
        }

    }

    internal static class HttpExtensions
    {
        private static MethodInfo _addInternalMethod;

        internal static void AddInternal(this WebHeaderCollection headers, string key, string value)
        {
            if (_addInternalMethod == null)
            {
                MethodInfo mi = typeof(WebHeaderCollection).GetMethod(
                    "AddInternal",
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(string), typeof(string) },
                    null);
                _addInternalMethod = mi;
            }

            _addInternalMethod.Invoke(headers, new object[] { key, value });
        }
    }
}
