// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.ServiceClient
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aliyun.OpenServices.Common.Communication
{
  /// <summary>
  /// The default implementation of <see cref="T:Aliyun.OpenServices.Common.Communication.IServiceClient"/>.
  /// 
  /// </summary>
  internal abstract class ServiceClient : IServiceClient
  {
    private ClientConfiguration _configuration;

    protected ClientConfiguration Configuration
    {
      get
      {
        return this._configuration;
      }
    }

    protected ServiceClient(ClientConfiguration configuration)
    {
      Debug.Assert(configuration != null);
      this._configuration = (ClientConfiguration) configuration.Clone();
    }

    public static ServiceClient Create(ClientConfiguration configuration)
    {
      return (ServiceClient) new ServiceClientImpl(configuration);
    }

    public ServiceResponse Send(ServiceRequest request, ExecutionContext context)
    {
      Debug.Assert(request != null);
      ServiceClient.SignRequest(request, context);
      ServiceResponse response = this.SendCore(request, context);
      ServiceClient.HandleResponse(response, context.ResponseHandlers);
      return response;
    }

    public IAsyncResult BeginSend(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state)
    {
      Debug.Assert(request != null);
      ServiceClient.SignRequest(request, context);
      return this.BeginSendCore(request, context, callback, state);
    }

    public ServiceResponse EndSend(IAsyncResult aysncResult)
    {
      AsyncResult<ServiceResponse> asyncResult = aysncResult as AsyncResult<ServiceResponse>;
      Debug.Assert(asyncResult != null);
      try
      {
        ServiceResponse result = asyncResult.GetResult();
        asyncResult.Dispose();
        return result;
      }
      catch (ObjectDisposedException ex)
      {
        throw new InvalidOperationException(Resources.ExceptionEndOperationHasBeenCalled);
      }
    }

    protected abstract ServiceResponse SendCore(ServiceRequest request, ExecutionContext context);

    protected abstract IAsyncResult BeginSendCore(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state);

    private static void SignRequest(ServiceRequest request, ExecutionContext context)
    {
      if (context.Signer == null)
        return;
      context.Signer.Sign(request, context.Credentials);
    }

    protected static void HandleResponse(ServiceResponse response, IList<IResponseHandler> handlers)
    {
      foreach (IResponseHandler responseHandler in (IEnumerable<IResponseHandler>) handlers)
        responseHandler.Handle(response);
    }
  }
}
