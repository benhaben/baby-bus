// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.ResponseDeserializer`2
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of ResponseDeserializer.
  /// 
  /// </summary>
  internal abstract class ResponseDeserializer<TResult, TModel> : IDeserializer<ServiceResponse, TResult>
  {
    protected IDeserializer<Stream, TModel> ContentDeserializer { get; private set; }

    public ResponseDeserializer(IDeserializer<Stream, TModel> contentDeserializer)
    {
      this.ContentDeserializer = contentDeserializer;
    }

    public abstract TResult Deserialize(ServiceResponse response);
  }
}
