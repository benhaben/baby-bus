// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.CopyObjectCommand
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
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of CopyObjectCommand.
  /// 
  /// </summary>
  internal class CopyObjectCommand : OssCommand<CopyObjectResult>
  {
    private CopyObjectRequest _copyObjectRequset;

    protected override string Bucket
    {
      get
      {
        return this._copyObjectRequset.DestinationBucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._copyObjectRequset.DestinationKey;
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Put;
      }
    }

    protected override IDictionary<string, string> Headers
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        this._copyObjectRequset.Populate((IDictionary<string, string>) dictionary);
        return (IDictionary<string, string>) dictionary;
      }
    }

    private CopyObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, CopyObjectResult> deserializer, CopyObjectRequest copyObjectRequest)
      : base(client, endpoint, context, deserializer)
    {
      Debug.Assert(copyObjectRequest != null);
      this._copyObjectRequset = copyObjectRequest;
    }

    public static CopyObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, CopyObjectRequest copyObjectRequest)
    {
      if (string.IsNullOrEmpty(copyObjectRequest.SourceBucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceBucketName");
      if (string.IsNullOrEmpty(copyObjectRequest.SourceKey))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceKey");
      if (string.IsNullOrEmpty(copyObjectRequest.DestinationBucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationBucketName");
      if (string.IsNullOrEmpty(copyObjectRequest.DestinationKey))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationKey");
      if (!OssUtils.IsBucketNameValid(copyObjectRequest.SourceBucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "sourceBucketName");
      if (!OssUtils.IsObjectKeyValid(copyObjectRequest.SourceKey))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "sourceKey");
      if (!OssUtils.IsBucketNameValid(copyObjectRequest.DestinationBucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "destinationBucketName");
      if (!OssUtils.IsObjectKeyValid(copyObjectRequest.DestinationKey))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "destinationKey");
      else
        return new CopyObjectCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateCopyObjectResultDeserializer(), copyObjectRequest);
    }
  }
}
