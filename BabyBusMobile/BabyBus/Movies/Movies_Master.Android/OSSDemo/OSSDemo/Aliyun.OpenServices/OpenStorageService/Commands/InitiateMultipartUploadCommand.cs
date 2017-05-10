// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.InitiateMultipartUploadCommand
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
  /// Description of InitiateMultipartUploadCommand.
  /// 
  /// </summary>
  internal class InitiateMultipartUploadCommand : OssCommand<InitiateMultipartUploadResult>
  {
    private InitiateMultipartUploadRequest _initiateMultipartUploadRequest;

    protected override string Bucket
    {
      get
      {
        return this._initiateMultipartUploadRequest.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._initiateMultipartUploadRequest.Key;
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
        parameters["uploads"] = (string) null;
        return parameters;
      }
    }

    protected override Stream Content
    {
      get
      {
        return (Stream) new MemoryStream(new byte[0]);
      }
    }

    protected override IDictionary<string, string> Headers
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        if (this._initiateMultipartUploadRequest.ObjectMetaData != null)
          this._initiateMultipartUploadRequest.ObjectMetaData.Populate((IDictionary<string, string>) dictionary);
        return (IDictionary<string, string>) dictionary;
      }
    }

    private InitiateMultipartUploadCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, InitiateMultipartUploadResult> deserializeMethod, InitiateMultipartUploadRequest initiateMultipartUploadRequest)
      : base(client, endpoint, context, deserializeMethod)
    {
      Debug.Assert(initiateMultipartUploadRequest != null);
      this._initiateMultipartUploadRequest = initiateMultipartUploadRequest;
    }

    public static InitiateMultipartUploadCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, InitiateMultipartUploadRequest initiateMultipartUploadRequest)
    {
      if (initiateMultipartUploadRequest == null)
        throw new ArgumentNullException("initiateMultipartUploadRequest");
      if (string.IsNullOrEmpty(initiateMultipartUploadRequest.BucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(initiateMultipartUploadRequest.Key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (!OssUtils.IsBucketNameValid(initiateMultipartUploadRequest.BucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(initiateMultipartUploadRequest.Key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      else
        return new InitiateMultipartUploadCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateInitiateMultipartUploadResultDeserializer(), initiateMultipartUploadRequest);
    }
  }
}
