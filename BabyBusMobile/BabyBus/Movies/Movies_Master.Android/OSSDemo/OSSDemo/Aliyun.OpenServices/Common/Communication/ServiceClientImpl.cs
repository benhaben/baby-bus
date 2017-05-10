// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.ServiceClientImpl
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common;
using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Aliyun.OpenServices.Common.Communication
{
    /// <summary>
    /// An default <see cref="T:Aliyun.OpenServices.Common.Communication.ServiceClient"/> implementation that
    ///             communicates with Aliyun Services over the HTTP protocol.
    /// 
    /// </summary>
    internal class ServiceClientImpl : ServiceClient
    {
        public ServiceClientImpl (ClientConfiguration configuration)
            : base (configuration)
        {
        }

        protected override ServiceResponse SendCore (ServiceRequest serviceRequest, ExecutionContext context)
        {
            HttpWebRequest webRequest = HttpFactory.CreateWebRequest (serviceRequest, this.Configuration);
            ServiceClientImpl.SetRequestContent (webRequest, serviceRequest, false, (Action)null);
            try {
                return (ServiceResponse)new ServiceClientImpl.ResponseImpl (webRequest.GetResponse () as HttpWebResponse);
            } catch (WebException ex) {
                return ServiceClientImpl.HandleException (ex);
            }
        }

        protected override IAsyncResult BeginSendCore (ServiceRequest serviceRequest, ExecutionContext context, AsyncCallback callback, object state)
        {
            HttpWebRequest request = HttpFactory.CreateWebRequest (serviceRequest, this.Configuration);
            ServiceClientImpl.HttpAsyncResult asyncResult = new ServiceClientImpl.HttpAsyncResult (callback, state);
            asyncResult.WebRequest = request;
            asyncResult.Context = context;
            ServiceClientImpl.SetRequestContent (request, serviceRequest, true, (Action)(() => request.BeginGetResponse (new AsyncCallback (this.OnGetResponseCompleted), (object)asyncResult)));
            return (IAsyncResult)asyncResult;
        }

        private void OnGetResponseCompleted (IAsyncResult ar)
        {
            ServiceClientImpl.HttpAsyncResult httpAsyncResult = ar.AsyncState as ServiceClientImpl.HttpAsyncResult;
            Debug.Assert (httpAsyncResult != null && httpAsyncResult.WebRequest != null);
            try {
                ServiceResponse serviceResponse = (ServiceResponse)new ServiceClientImpl.ResponseImpl (httpAsyncResult.WebRequest.EndGetResponse (ar) as HttpWebResponse);
                ServiceClient.HandleResponse (serviceResponse, httpAsyncResult.Context.ResponseHandlers);
                httpAsyncResult.Complete (serviceResponse);
            } catch (WebException ex1) {
                try {
                    ServiceResponse serviceResponse = ServiceClientImpl.HandleException (ex1);
                    ServiceClient.HandleResponse (serviceResponse, httpAsyncResult.Context.ResponseHandlers);
                    ((WebRequest)httpAsyncResult.WebRequest).Abort ();
                    httpAsyncResult.Complete (serviceResponse);
                } catch (Exception ex2) {
                    ((WebRequest)httpAsyncResult.WebRequest).Abort ();
                    ((AsyncResult)httpAsyncResult).Complete (ex2);
                }
            } catch (Exception ex) {
                ((WebRequest)httpAsyncResult.WebRequest).Abort ();
                ((AsyncResult)httpAsyncResult).Complete (ex);
            }
        }

        private static void SetRequestContent (HttpWebRequest webRequest, ServiceRequest serviceRequest, bool async, Action asyncCallback)
        {
            Stream data = serviceRequest.BuildRequestContent ();
            if (data == null || serviceRequest.Method != HttpMethod.Put && serviceRequest.Method != HttpMethod.Post) {
                if (!async)
                    return;
                asyncCallback ();
            } else {
                long num1 = -1L;
                if (serviceRequest.Headers.ContainsKey ("Content-Length"))
                    num1 = long.Parse (serviceRequest.Headers ["Content-Length"]);
                long num2 = data.Length - data.Position;
                webRequest.ContentLength = num1 < 0L || num1 > num2 ? num2 : num1;
                if (async) {
                    Debug.Assert (asyncCallback != null);
                    webRequest.BeginGetRequestStream ((AsyncCallback)(ar => {
                        using (Stream requestStream = ((WebRequest)webRequest).EndGetRequestStream (ar))
                            IOUtils.WriteTo (data, requestStream, webRequest.ContentLength);
                        asyncCallback ();
                    }), (object)null);
                } else {
                    using (Stream requestStream = ((WebRequest)webRequest).GetRequestStream ()) {
                        try {
                            IOUtils.WriteTo (data, requestStream, webRequest.ContentLength);
                        } catch (Exception ex) {
                        }
                    }
                }
            }
        }

        private static ServiceResponse HandleException (WebException ex)
        {
            if (!(ex.Response is HttpWebResponse))
                throw ex;
            else
                return (ServiceResponse)new ServiceClientImpl.ResponseImpl (ex);
        }

        /// <summary>
        /// Represents the async operation of requests in <see cref="T:Aliyun.OpenServices.Common.Communication.ServiceClientImpl"/>.
        /// 
        /// </summary>
        private class HttpAsyncResult : AsyncResult<ServiceResponse>
        {
            public HttpWebRequest WebRequest { get; set; }

            public ExecutionContext Context { get; set; }

            public HttpAsyncResult (AsyncCallback callback, object state)
                : base (callback, state)
            {
            }
        }

        /// <summary>
        /// Represents the response data of <see cref="T:Aliyun.OpenServices.Common.Communication.ServiceClientImpl"/> requests.
        /// 
        /// </summary>
        private class ResponseImpl : ServiceResponse
        {
            private bool _disposed;
            private HttpWebResponse _response;
            private Exception _failure;
            private IDictionary<string, string> _headers;

            public override HttpStatusCode StatusCode {
                get {
                    return this._response.StatusCode;
                }
            }

            public override Exception Failure {
                get {
                    return this._failure;
                }
            }

            public override IDictionary<string, string> Headers {
                get {
                    this.ThrowIfObjectDisposed ();
                    if (this._headers == null)
                        this._headers = ServiceClientImpl.ResponseImpl.GetResponseHeaders (this._response);
                    return this._headers;
                }
            }

            public override Stream Content {
                get {
                    this.ThrowIfObjectDisposed ();
                    try {
                        return this._response != null ? this._response.GetResponseStream () : (Stream)null;
                    } catch (ProtocolViolationException ex) {
                        throw new InvalidOperationException (ex.Message, (Exception)ex);
                    }
                }
            }

            public ResponseImpl (HttpWebResponse httpWebResponse)
            {
                Debug.Assert (httpWebResponse != null);
                this._response = httpWebResponse;
                Debug.Assert (this.IsSuccessful (), "This constructor only allows a successfull response.");
            }

            public ResponseImpl (WebException failure)
            {
                Debug.Assert (failure != null);
                HttpWebResponse httpWebResponse = failure.Response as HttpWebResponse;
                Debug.Assert (httpWebResponse != null);
                this._failure = (Exception)failure;
                this._response = httpWebResponse;
                Debug.Assert (!this.IsSuccessful (), "This constructor only allows a failed response.");
            }

            private static IDictionary<string, string> GetResponseHeaders (HttpWebResponse response)
            {
                WebHeaderCollection headers = response.Headers;
                Dictionary<string, string> dictionary = new Dictionary<string, string> (headers.Count);
                for (int index = 0; index < headers.Count; ++index) {
                    string str = headers.Keys [index];
                    string text = headers.Get (str);
                    dictionary.Add (str, HttpUtils.ReEncode (text, "iso-8859-1", "utf-8"));
                }
                return (IDictionary<string, string>)dictionary;
            }

            protected override void Dispose (bool disposing)
            {
                base.Dispose (disposing);
                if (this._disposed || !disposing)
                    return;
                if (this._response != null) {
                    this._response.Close ();
                    this._response = (HttpWebResponse)null;
                }
                this._disposed = true;
            }

            private void ThrowIfObjectDisposed ()
            {
                if (this._disposed)
                    throw new ObjectDisposedException (this.GetType ().Name);
            }
        }
    }
}
