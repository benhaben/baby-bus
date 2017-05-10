// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.SetBucketCorsCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  internal class SetBucketCorsCommand : OssCommand
  {
    private string _bucketName;
    private SetBucketCorsRequest _setBucketCorsRequest;

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Put;
      }
    }

    protected override string Bucket
    {
      get
      {
        return this._bucketName;
      }
    }

    protected override Stream Content
    {
      get
      {
        return SerializerFactory.GetFactory().CreateSetBucketCorsRequestSerializer().Serialize(this._setBucketCorsRequest);
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        return (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "cors",
            (string) null
          }
        };
      }
    }

    private SetBucketCorsCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, SetBucketCorsRequest setBucketCorsRequest)
      : base(client, endpoint, context)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      this._bucketName = bucketName;
      this._setBucketCorsRequest = setBucketCorsRequest;
    }

    public static SetBucketCorsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, SetBucketCorsRequest setBucketCorsRequest)
    {
      return new SetBucketCorsCommand(client, endpoint, context, bucketName, setBucketCorsRequest);
    }
  }
}
