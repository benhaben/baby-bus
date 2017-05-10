// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.IServiceClient
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;

namespace Aliyun.OpenServices.Common.Communication
{
  /// <summary>
  /// Represent the channel that communicates with an Aliyun Open Service.
  /// 
  /// </summary>
  internal interface IServiceClient
  {
    /// <summary>
    /// Sends a request to the service.
    /// 
    /// </summary>
    /// <param name="request">The request data.</param><param name="context">The execution context.</param>
    /// <returns>
    /// The response data.
    /// </returns>
    ServiceResponse Send(ServiceRequest request, ExecutionContext context);

    /// <summary>
    /// Begins to send a request to the service asynchronously.
    /// 
    /// </summary>
    /// <param name="request">The request data.</param><param name="context">The execution context.</param><param name="callback">User callback.</param><param name="state">User state.</param>
    /// <returns>
    /// An instance of <see cref="T:System.IAsyncResult"/>.
    /// </returns>
    IAsyncResult BeginSend(ServiceRequest request, ExecutionContext context, AsyncCallback callback, object state);

    /// <summary>
    /// Ends the asynchronous operation.
    /// 
    /// </summary>
    /// <param name="asyncResult">An instance of <see cref="T:System.IAsyncResult"/>.</param>
    /// <returns>
    /// The response data.
    /// </returns>
    ServiceResponse EndSend(IAsyncResult asyncResult);
  }
}
