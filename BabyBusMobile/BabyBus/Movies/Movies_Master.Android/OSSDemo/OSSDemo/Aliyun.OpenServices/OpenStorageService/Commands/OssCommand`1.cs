// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.OssCommand`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using System;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  internal abstract class OssCommand<T> : OssCommand
  {
    private IDeserializer<ServiceResponse, T> _deserializer;

    protected virtual bool LeaveResponseOpen
    {
      get
      {
        return false;
      }
    }

    public OssCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, T> deserializer)
      : base(client, endpoint, context)
    {
      Debug.Assert(deserializer != null);
      this._deserializer = deserializer;
    }

    public T Execute()
    {
      ServiceResponse input = base.Execute();
      try
      {
        return this._deserializer.Deserialize(input);
      }
      catch (ResponseDeserializationException ex)
      {
        throw ExceptionFactory.CreateInvalidResponseException((Exception) ex);
      }
      finally
      {
        if (!this.LeaveResponseOpen)
          input.Dispose();
      }
    }
  }
}
