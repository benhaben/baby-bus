// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.GetBucketLoggingResultDeserializer
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
  internal class GetBucketLoggingResultDeserializer : ResponseDeserializer<BucketLoggingResult, SetBucketLoggingRequestModel>
  {
    public GetBucketLoggingResultDeserializer(IDeserializer<Stream, SetBucketLoggingRequestModel> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override BucketLoggingResult Deserialize(ServiceResponse response)
    {
      SetBucketLoggingRequestModel loggingRequestModel = this.ContentDeserializer.Deserialize(response.Content);
      return new BucketLoggingResult()
      {
        TargetBucket = loggingRequestModel.LoggingEnabled.TargetBucket,
        TargetPrefix = loggingRequestModel.LoggingEnabled.TargetPrefix
      };
    }
  }
}
