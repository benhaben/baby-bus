// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.RetryableServiceClient
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common;
using Aliyun.OpenServices.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace Aliyun.OpenServices.Common.Communication
{
  /// <summary>
  /// Represents the <see cref="T:Aliyun.OpenServices.Common.Communication.IServiceClient"/> implementation
  ///             that can automatically retry the request operations if they are failed
  ///             due to specific exceptions.
  /// 
  /// </summary>
  internal class RetryableServiceClient : IServiceClient
  {
    private const int _defaultRetryPauseScale = 300;
    private IServiceClient _innerClient;

    public Func<Exception, bool> ShouldRetryCallback { get; set; }

    /// <summary>
    /// Gets or sets the max retry times on error.
    /// 
    /// </summary>
    public int MaxErrorRetry { get; set; }

    public RetryableServiceClient(IServiceClient innerClient)
    {
      Debug.Assert(innerClient != null);
      this._innerClient = innerClient;
      this.MaxErrorRetry = 3;
    }

    public ServiceResponse Send(ServiceRequest request, ExecutionContext context)
    {
      return this.SendImpl(request, context, 0);
    }

    private ServiceResponse SendImpl(ServiceRequest request, ExecutionContext context, int retries)
    {
      long offset = -1L;
      try
      {
        if (request.Content != null && request.Content.CanSeek)
          offset = request.Content.Position;
        return this._innerClient.Send(request, context);
      }
      catch (Exception ex)
      {
        if (!this.ShouldRetry(request, ex, retries))
          throw ex;
        if (offset >= 0L && request.Content.CanSeek)
          request.Content.Seek(offset, SeekOrigin.Begin);
        RetryableServiceClient.Pause(retries);
        return this.SendImpl(request, context, ++retries);
      }
    }

    public IAsyncResult BeginSend(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state)
    {
      RetryableAsyncResult asyncResult = new RetryableAsyncResult(callback, state, request, context);
      this.BeginSendImpl(request, context, asyncResult);
      return (IAsyncResult) asyncResult;
    }

    private void BeginSendImpl(ServiceRequest request, ExecutionContext context, RetryableAsyncResult asyncResult)
    {
      if (asyncResult.InnerAsyncResult != null)
        asyncResult.InnerAsyncResult.Dispose();
      asyncResult.InnerAsyncResult = this._innerClient.BeginSend(request, context, new AsyncCallback(this.OnBeginSendCompleted), (object) asyncResult) as AsyncResult;
    }

    private void OnBeginSendCompleted(IAsyncResult ar)
    {
      RetryableAsyncResult asyncResult = ar.AsyncState as RetryableAsyncResult;
      try
      {
        ServiceResponse result = this._innerClient.EndSend(ar);
        asyncResult.Complete(result);
      }
      catch (Exception ex)
      {
        if (asyncResult.OriginalContentPosition >= 0L)
          asyncResult.Request.Content.Seek(asyncResult.OriginalContentPosition, SeekOrigin.Begin);
        if (this.ShouldRetry(asyncResult.Request, ex, asyncResult.Retries))
        {
          RetryableServiceClient.Pause(asyncResult.Retries++);
          this.BeginSendImpl(asyncResult.Request, asyncResult.Context, asyncResult);
        }
        else
          ((AsyncResult) asyncResult).Complete(ex);
      }
    }

    public ServiceResponse EndSend(IAsyncResult ar)
    {
      RetryableAsyncResult retryableAsyncResult = ar as RetryableAsyncResult;
      Debug.Assert(ar != null);
      try
      {
        ServiceResponse result = retryableAsyncResult.GetResult();
        retryableAsyncResult.Dispose();
        return result;
      }
      catch (ObjectDisposedException ex)
      {
        throw new InvalidOperationException(Resources.ExceptionEndOperationHasBeenCalled);
      }
    }

    private bool ShouldRetry(ServiceRequest request, Exception ex, int retries)
    {
      if (retries > this.MaxErrorRetry || !request.IsRepeatable)
        return false;
      WebException webException = ex as WebException;
      if (webException != null)
      {
        HttpWebResponse httpWebResponse = webException.Response as HttpWebResponse;
        if (httpWebResponse != null && (httpWebResponse.StatusCode == HttpStatusCode.ServiceUnavailable || httpWebResponse.StatusCode == HttpStatusCode.InternalServerError))
          return true;
      }
      return this.ShouldRetryCallback != null && this.ShouldRetryCallback(ex);
    }

    private static void Pause(int retries)
    {
      int num = 300;
      Thread.Sleep((int) Math.Pow(2.0, (double) retries) * num);
    }
  }
}
