// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.AbortMultipartUploadCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of AbortMultipartUploadCommand.
  /// 
  /// </summary>
  internal class AbortMultipartUploadCommand : OssCommand
  {
    private AbortMultipartUploadRequest _abortMultipartUploadRequest;

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
        return this._abortMultipartUploadRequest.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._abortMultipartUploadRequest.Key;
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        IDictionary<string, string> parameters = base.Parameters;
        parameters["uploadId"] = this._abortMultipartUploadRequest.UploadId;
        return parameters;
      }
    }

    private AbortMultipartUploadCommand(IServiceClient client, Uri endpoint, ExecutionContext context, AbortMultipartUploadRequest abortMultipartUploadRequest)
      : base(client, endpoint, context)
    {
      Debug.Assert(abortMultipartUploadRequest != null);
      this._abortMultipartUploadRequest = abortMultipartUploadRequest;
    }

    public static AbortMultipartUploadCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, AbortMultipartUploadRequest abortMultipartUploadRequest)
    {
      if (abortMultipartUploadRequest == null)
        throw new ArgumentNullException("abortMultipartUploadRequest");
      if (string.IsNullOrEmpty(abortMultipartUploadRequest.BucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(abortMultipartUploadRequest.Key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (string.IsNullOrEmpty(abortMultipartUploadRequest.UploadId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");
      if (!OssUtils.IsBucketNameValid(abortMultipartUploadRequest.BucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(abortMultipartUploadRequest.Key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      else
        return new AbortMultipartUploadCommand(client, endpoint, context, abortMultipartUploadRequest);
    }
  }
}
