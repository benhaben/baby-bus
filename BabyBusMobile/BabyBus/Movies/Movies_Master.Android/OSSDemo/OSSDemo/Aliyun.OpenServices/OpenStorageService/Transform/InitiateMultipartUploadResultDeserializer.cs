// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.InitiateMultipartUploadResultDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of InitiateMultipartUploadResultDeserializer.
  /// 
  /// </summary>
  internal class InitiateMultipartUploadResultDeserializer : ResponseDeserializer<InitiateMultipartUploadResult, InitiateMultipartResult>
  {
    public InitiateMultipartUploadResultDeserializer(IDeserializer<Stream, InitiateMultipartResult> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override InitiateMultipartUploadResult Deserialize(ServiceResponse response)
    {
      InitiateMultipartResult initiateMultipartResult = this.ContentDeserializer.Deserialize(response.Content);
      return new InitiateMultipartUploadResult()
      {
        BucketName = initiateMultipartResult.Bucket,
        Key = initiateMultipartResult.Key,
        UploadId = initiateMultipartResult.UploadId
      };
    }
  }
}
