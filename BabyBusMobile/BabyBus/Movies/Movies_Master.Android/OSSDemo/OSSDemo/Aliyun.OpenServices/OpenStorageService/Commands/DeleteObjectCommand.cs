// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.DeleteObjectCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  internal class DeleteObjectCommand : OssCommand
  {
    private string _bucketName;
    private string _key;

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Delete;
      }
    }

    protected override string Bucket
    {
      get
      {
        return this._bucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._key;
      }
    }

    private DeleteObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, string key)
      : base(client, endpoint, context)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      this._bucketName = bucketName;
      this._key = key;
    }

    public static DeleteObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, string key)
    {
      return new DeleteObjectCommand(client, endpoint, context, bucketName, key);
    }
  }
}
