// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.OtsExceptionHandler
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenTableService.Model;
using System;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  internal class OtsExceptionHandler : ResponseHandler
  {
    public override void Handle(ServiceResponse response)
    {
      base.Handle(response);
      if (response.IsSuccessful())
        return;
      ErrorResult errorResult = (ErrorResult) null;
      try
      {
        IDeserializer<Stream, ErrorResult> deserializer = DeserializerFactory.CreateDeserializer<ErrorResult>(response.Headers.ContainsKey("Content-Type") ? response.Headers["Content-Type"] : (string) null);
        if (deserializer == null)
          response.EnsureSuccessful();
        errorResult = deserializer.Deserialize(response.Content);
      }
      catch (ResponseDeserializationException ex)
      {
        response.EnsureSuccessful();
      }
      throw ExceptionFactory.CreateException(errorResult, (Exception) null);
    }
  }
}
