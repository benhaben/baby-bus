// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.OtsCommand`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Base class for OTS commands that return response objects.
  /// 
  /// </summary>
  internal abstract class OtsCommand<T> : OtsCommand
  {
    protected OtsCommand(IServiceClient client, Uri endpoint, ExecutionContext context)
      : base(client, endpoint, context)
    {
    }

    /// <summary>
    /// Executes the command.
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public T Execute()
    {
      ServiceRequest request = this.CreateRequest();
      try
      {
        using (ServiceResponse response = this.Client.Send(request, this.Context))
          return OtsCommand<T>.GetResultFromResponse(response);
      }
      finally
      {
        request.Dispose();
      }
    }

    public static T EndExecute(IServiceClient client, IAsyncResult asyncResult)
    {
      Debug.Assert(client != null);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OtsCommand.OtsCommandAsyncResult commandAsyncResult = (OtsCommand.OtsCommandAsyncResult) asyncResult;
      ServiceRequest serviceRequest = commandAsyncResult.ServiceRequest;
      try
      {
        using (ServiceResponse response = client.EndSend(commandAsyncResult.AsyncResult))
          return OtsCommand<T>.GetResultFromResponse(response);
      }
      finally
      {
        serviceRequest.Dispose();
      }
    }

    private static T GetResultFromResponse(ServiceResponse response)
    {
      try
      {
        IDeserializer<Stream, T> deserializer = DeserializerFactory.CreateDeserializer<T>(response.Headers.ContainsKey("Content-Type") ? response.Headers["Content-Type"] : (string) null);
        if (deserializer == null)
          throw ExceptionFactory.CreateInvalidResponseException(response, OtsExceptions.ResponseDataIncomplete, (Exception) null);
        else
          return deserializer.Deserialize(response.Content);
      }
      catch (ResponseDeserializationException ex)
      {
        throw ExceptionFactory.CreateInvalidResponseException(response, OtsExceptions.ResponseDataIncomplete, (Exception) ex);
      }
    }
  }
}
