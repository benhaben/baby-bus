// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.GetCorsResponseDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  internal class GetCorsResponseDeserializer : ResponseDeserializer<IList<CORSRule>, SetBucketCorsRequestModel>
  {
    public GetCorsResponseDeserializer(IDeserializer<Stream, SetBucketCorsRequestModel> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override IList<CORSRule> Deserialize(ServiceResponse response)
    {
      SetBucketCorsRequestModel corsRequestModel = this.ContentDeserializer.Deserialize(response.Content);
      IList<CORSRule> list = (IList<CORSRule>) new List<CORSRule>();
      foreach (SetBucketCorsRequestModel.CORSRuleModel corsRuleModel in corsRequestModel.CORSRuleModels)
        list.Add(new CORSRule()
        {
          AllowedHeaders = (IList<string>) corsRuleModel.AllowedHeaders,
          AllowedMethods = (IList<string>) corsRuleModel.AllowedMethods,
          AllowedOrigins = (IList<string>) corsRuleModel.AllowedOrigins,
          ExposeHeaders = (IList<string>) corsRuleModel.ExposeHeaders,
          MaxAgeSeconds = corsRuleModel.MaxAgeSeconds
        });
      return list;
    }
  }
}
