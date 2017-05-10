// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.OptionsObjectCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  internal class OptionsObjectCommand : OssCommand
  {
    private string _bucketName;
    private string _key;
    private OptionsRequest _optionsRequest;

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Options;
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

    protected override IDictionary<string, string> Headers
    {
      get
      {
        return (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Origin",
            this._optionsRequest.Origin
          },
          {
            "Access-Control-Request-Headers",
            this._optionsRequest.AccessControlRequestHeaders
          },
          {
            "Access-Control-Request-Method",
            this._optionsRequest.AccessControlRequestMethod
          }
        };
      }
    }

    private OptionsObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, OptionsRequest optionsRequest)
      : base(client, endpoint, context)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(optionsRequest.Key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "OptionsRequest.Key");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(optionsRequest.Key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "OptionsRequest.Key");
      if (optionsRequest == null)
        throw new ArgumentNullException("optionsRequest");
      this._bucketName = bucketName;
      this._key = optionsRequest.Key;
      this._optionsRequest = optionsRequest;
    }

    public static OptionsObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, OptionsRequest optionsRequest)
    {
      return new OptionsObjectCommand(client, endpoint, context, bucketName, optionsRequest);
    }
  }
}
