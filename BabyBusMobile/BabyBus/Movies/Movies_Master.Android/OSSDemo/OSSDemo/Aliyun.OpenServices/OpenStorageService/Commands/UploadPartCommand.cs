// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.UploadPartCommand
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
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of UploadPartCommand.
  /// 
  /// </summary>
  internal class UploadPartCommand : OssCommand<UploadPartResult>
  {
    private UploadPartRequest _uploadPartRequest;

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
        return this._uploadPartRequest.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._uploadPartRequest.Key;
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        IDictionary<string, string> parameters = base.Parameters;
        parameters["partNumber"] = this._uploadPartRequest.PartNumber.ToString();
        parameters["uploadId"] = this._uploadPartRequest.UploadId;
        return parameters;
      }
    }

    protected override IDictionary<string, string> Headers
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Content-Length"] = this._uploadPartRequest.PartSize.ToString();
        return (IDictionary<string, string>) dictionary;
      }
    }

    protected override Stream Content
    {
      get
      {
        return this._uploadPartRequest.InputStream;
      }
    }

    protected override bool LeaveRequestOpen
    {
      get
      {
        return true;
      }
    }

    private UploadPartCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, UploadPartResult> deserializer, UploadPartRequest uploadPartRequest)
      : base(client, endpoint, context, deserializer)
    {
      Debug.Assert(uploadPartRequest != null);
      this._uploadPartRequest = uploadPartRequest;
    }

    private static bool IsPartNumberInRange(int? partNumber)
    {
      int? nullable = partNumber;
      int num;
      if ((nullable.GetValueOrDefault() <= 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        nullable = partNumber;
        num = nullable.GetValueOrDefault() > 10000 ? 0 : (nullable.HasValue ? 1 : 0);
      }
      else
        num = 0;
      return num != 0;
    }

    public static UploadPartCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, UploadPartRequest uploadPartRequest)
    {
      if (uploadPartRequest == null)
        throw new ArgumentNullException("uploadPartRequest");
      if (string.IsNullOrEmpty(uploadPartRequest.BucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(uploadPartRequest.Key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (string.IsNullOrEmpty(uploadPartRequest.UploadId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");
      if (!uploadPartRequest.PartNumber.HasValue)
        throw new ArgumentNullException("partNumber");
      if (!uploadPartRequest.PartSize.HasValue)
        throw new ArgumentNullException("partSize");
      if (!OssUtils.IsBucketNameValid(uploadPartRequest.BucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(uploadPartRequest.Key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      if (uploadPartRequest.InputStream == null)
        throw new ArgumentNullException("inputStream");
      long? partSize = uploadPartRequest.PartSize;
      int num;
      if ((partSize.GetValueOrDefault() >= 0L ? 0 : (partSize.HasValue ? 1 : 0)) == 0)
      {
        partSize = uploadPartRequest.PartSize;
        num = (partSize.GetValueOrDefault() <= 5368709120L ? 0 : (partSize.HasValue ? 1 : 0)) == 0 ? 1 : 0;
      }
      else
        num = 0;
      if (num == 0)
        throw new ArgumentOutOfRangeException("partSize");
      if (!UploadPartCommand.IsPartNumberInRange(uploadPartRequest.PartNumber))
        throw new ArgumentOutOfRangeException("partNumber");
      else
        return new UploadPartCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateUploadPartResultDeserializer(uploadPartRequest.PartNumber.Value), uploadPartRequest);
    }
  }
}
