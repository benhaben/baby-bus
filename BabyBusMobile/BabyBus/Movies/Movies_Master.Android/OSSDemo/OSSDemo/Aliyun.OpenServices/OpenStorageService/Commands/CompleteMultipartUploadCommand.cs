// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.CompleteMultipartUploadCommand
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
  /// Description of CompleteMultipartUploadCommand.
  /// 
  /// </summary>
  internal class CompleteMultipartUploadCommand : OssCommand<CompleteMultipartUploadResult>
  {
    private CompleteMultipartUploadRequest _completeMultipartUploadRequest;

    protected override string Bucket
    {
      get
      {
        return this._completeMultipartUploadRequest.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._completeMultipartUploadRequest.Key;
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        IDictionary<string, string> parameters = base.Parameters;
        parameters["uploadId"] = this._completeMultipartUploadRequest.UploadId;
        return parameters;
      }
    }

    protected override Stream Content
    {
      get
      {
        return SerializerFactory.GetFactory().CreateCompleteUploadRequestSerializer().Serialize(this._completeMultipartUploadRequest);
      }
    }

    private CompleteMultipartUploadCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, CompleteMultipartUploadResult> deserializeMethod, CompleteMultipartUploadRequest completeMultipartUploadRequest)
      : base(client, endpoint, context, deserializeMethod)
    {
      Debug.Assert(completeMultipartUploadRequest != null);
      this._completeMultipartUploadRequest = completeMultipartUploadRequest;
    }

    public static CompleteMultipartUploadCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, CompleteMultipartUploadRequest completeMultipartUploadRequest)
    {
      if (completeMultipartUploadRequest == null)
        throw new ArgumentNullException("completeMultipartUploadRequest");
      if (string.IsNullOrEmpty(completeMultipartUploadRequest.BucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(completeMultipartUploadRequest.Key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (string.IsNullOrEmpty(completeMultipartUploadRequest.UploadId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");
      if (!OssUtils.IsBucketNameValid(completeMultipartUploadRequest.BucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(completeMultipartUploadRequest.Key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      else
        return new CompleteMultipartUploadCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateCompleteUploadResultDeserializer(), completeMultipartUploadRequest);
    }
  }
}
