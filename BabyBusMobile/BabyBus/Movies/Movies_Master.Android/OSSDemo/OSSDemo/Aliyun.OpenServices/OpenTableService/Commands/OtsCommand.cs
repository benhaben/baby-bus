// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.OtsCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Base class for OTS commands.
  /// 
  /// </summary>
  internal abstract class OtsCommand
  {
    public const string UrlEncodedContentType = "application/x-www-form-urlencoded; charset=utf-8";

    public Aliyun.OpenServices.Common.Communication.ExecutionContext Context { get; private set; }

    public IServiceClient Client { get; private set; }

    public Uri Endpoint { get; private set; }

    protected abstract HttpMethod Method { get; }

    protected abstract string ResourcePath { get; }

    protected OtsCommand(IServiceClient client, Uri endpoint, Aliyun.OpenServices.Common.Communication.ExecutionContext context)
    {
      Debug.Assert(client != null && endpoint != (Uri) null && context != null);
      this.Endpoint = endpoint;
      this.Client = client;
      this.Context = context;
    }

    public void Execute()
    {
      ServiceRequest request = this.CreateRequest();
      try
      {
        using (this.Client.Send(request, this.Context))
          ;
      }
      finally
      {
        request.Dispose();
      }
    }

    public IAsyncResult BeginExecute(AsyncCallback callback, object state)
    {
      ServiceRequest request = this.CreateRequest();
      OtsCommand.OtsCommandAsyncResult otsAsyncResult = new OtsCommand.OtsCommandAsyncResult();
      otsAsyncResult.ServiceRequest = request;
      IAsyncResult asyncResult = this.Client.BeginSend(request, this.Context, (AsyncCallback) (ar =>
      {
        if (callback == null)
          return;
        callback((IAsyncResult) otsAsyncResult);
      }), state);
      otsAsyncResult.AsyncResult = asyncResult;
      return (IAsyncResult) otsAsyncResult;
    }

    public static void EndExecute(IServiceClient client, IAsyncResult asyncResult)
    {
      Debug.Assert(client != null);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OtsCommand.OtsCommandAsyncResult commandAsyncResult = (OtsCommand.OtsCommandAsyncResult) asyncResult;
      ServiceRequest serviceRequest = commandAsyncResult.ServiceRequest;
      try
      {
        using (client.EndSend(commandAsyncResult.AsyncResult))
          ;
      }
      finally
      {
        serviceRequest.Dispose();
      }
    }

    protected ServiceRequest CreateRequest()
    {
      ServiceRequest serviceRequest = new ServiceRequest();
      serviceRequest.Method = this.Method;
      serviceRequest.Endpoint = this.Endpoint;
      serviceRequest.ResourcePath = this.ResourcePath;
      serviceRequest.Headers["Content-Type"] = "application/x-www-form-urlencoded; charset=utf-8";
      this.AddRequestParameters(serviceRequest.Parameters);
      return serviceRequest;
    }

    protected virtual void AddRequestParameters(IDictionary<string, string> parameters)
    {
    }

    protected class OtsCommandAsyncResult : IAsyncResult
    {
      public IAsyncResult AsyncResult { get; set; }

      public ServiceRequest ServiceRequest { get; set; }

      public bool IsCompleted
      {
        get
        {
          return this.AsyncResult.IsCompleted;
        }
      }

      public WaitHandle AsyncWaitHandle
      {
        get
        {
          return this.AsyncResult.AsyncWaitHandle;
        }
      }

      public object AsyncState
      {
        get
        {
          return this.AsyncResult.AsyncState;
        }
      }

      public bool CompletedSynchronously
      {
        get
        {
          return this.AsyncResult.CompletedSynchronously;
        }
      }
    }
  }
}
