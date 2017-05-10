// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.PutObjectCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Transform;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of PutObjectCommand.
  /// 
  /// </summary>
  internal class PutObjectCommand : OssCommand<PutObjectResult>
  {
    private OssObject _ossObject;

    protected override string Bucket
    {
      get
      {
        return this._ossObject.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._ossObject.Key;
      }
    }

    protected override bool LeaveRequestOpen
    {
      get
      {
        return true;
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Put;
      }
    }

    protected override Stream Content
    {
      get
      {
        return this._ossObject.Content;
      }
    }

    protected override IDictionary<string, string> Headers
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        this._ossObject.Metadata.Populate((IDictionary<string, string>) dictionary);
        return (IDictionary<string, string>) dictionary;
      }
    }

    private PutObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, PutObjectResult> deserializer, OssObject ossObject)
      : base(client, endpoint, context, deserializer)
    {
      this._ossObject = ossObject;
    }

    public static PutObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, string key, Stream content, ObjectMetadata metadata)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      if (content == null)
        throw new ArgumentNullException("content");
      if (metadata == null)
        throw new ArgumentNullException("metadata");
      return new PutObjectCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreatePutObjectReusltDeserializer(), new OssObject(key)
      {
        BucketName = bucketName,
        Content = content,
        Metadata = metadata
      });
    }
  }
}
