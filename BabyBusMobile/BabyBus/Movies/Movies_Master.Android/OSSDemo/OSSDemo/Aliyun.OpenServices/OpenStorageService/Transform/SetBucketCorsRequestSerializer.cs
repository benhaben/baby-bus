// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.SetBucketCorsRequestSerializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  internal class SetBucketCorsRequestSerializer : RequestSerializer<SetBucketCorsRequest, SetBucketCorsRequestModel>
  {
    public SetBucketCorsRequestSerializer(ISerializer<SetBucketCorsRequestModel, Stream> contentSerializer)
      : base(contentSerializer)
    {
    }

    public override Stream Serialize(SetBucketCorsRequest request)
    {
      SetBucketCorsRequestModel input = new SetBucketCorsRequestModel();
      input.CORSRuleModels = new SetBucketCorsRequestModel.CORSRuleModel[request.CORSRules.Count];
      for (int index1 = 0; index1 < request.CORSRules.Count; ++index1)
      {
        SetBucketCorsRequestModel.CORSRuleModel corsRuleModel = new SetBucketCorsRequestModel.CORSRuleModel();
        if (request.CORSRules[index1].AllowedHeaders != null)
        {
          corsRuleModel.AllowedHeaders = new string[request.CORSRules[index1].AllowedHeaders.Count];
          for (int index2 = 0; index2 < request.CORSRules[index1].AllowedHeaders.Count; ++index2)
            corsRuleModel.AllowedHeaders[index2] = request.CORSRules[index1].AllowedHeaders[index2];
        }
        if (request.CORSRules[index1].AllowedMethods != null)
        {
          corsRuleModel.AllowedMethods = new string[request.CORSRules[index1].AllowedMethods.Count];
          for (int index2 = 0; index2 < request.CORSRules[index1].AllowedMethods.Count; ++index2)
            corsRuleModel.AllowedMethods[index2] = request.CORSRules[index1].AllowedMethods[index2];
        }
        if (request.CORSRules[index1].AllowedOrigins != null)
        {
          corsRuleModel.AllowedOrigins = new string[request.CORSRules[index1].AllowedOrigins.Count];
          for (int index2 = 0; index2 < request.CORSRules[index1].AllowedOrigins.Count; ++index2)
            corsRuleModel.AllowedOrigins[index2] = request.CORSRules[index1].AllowedOrigins[index2];
        }
        if (request.CORSRules[index1].ExposeHeaders != null)
        {
          corsRuleModel.ExposeHeaders = new string[request.CORSRules[index1].ExposeHeaders.Count];
          for (int index2 = 0; index2 < request.CORSRules[index1].ExposeHeaders.Count; ++index2)
            corsRuleModel.ExposeHeaders[index2] = request.CORSRules[index1].ExposeHeaders[index2];
        }
        corsRuleModel.MaxAgeSeconds = request.CORSRules[index1].MaxAgeSeconds;
        input.CORSRuleModels[index1] = corsRuleModel;
      }
      return this.ContentSerializer.Serialize(input);
    }
  }
}
