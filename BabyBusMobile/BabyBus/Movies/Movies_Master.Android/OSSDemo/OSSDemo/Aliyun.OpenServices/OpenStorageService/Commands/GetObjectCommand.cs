// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.GetObjectCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Transform;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of GetObjectCommand.
  /// 
  /// </summary>
  internal class GetObjectCommand : OssCommand<OssObject>
  {
    private GetObjectRequest _request;

    protected override string Bucket
    {
      get
      {
        return this._request.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._request.Key;
      }
    }

    protected override IDictionary<string, string> Headers
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        this._request.Populate((IDictionary<string, string>) dictionary);
        return (IDictionary<string, string>) dictionary;
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        IDictionary<string, string> parameters = base.Parameters;
        this._request.ResponseHeaders.Populate(parameters);
        return parameters;
      }
    }

    protected override bool LeaveResponseOpen
    {
      get
      {
        return true;
      }
    }

    private GetObjectCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, OssObject> deserializer, GetObjectRequest request)
      : base(client, endpoint, context, deserializer)
    {
      this._request = request;
    }

    public static GetObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, GetObjectRequest request)
    {
      if (request == null)
        throw new ArgumentNullException("request");
      else
        return new GetObjectCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateGetObjectResultDeserializer(request), request);
    }

    public static GetObjectCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string bucketName, string key)
    {
      return GetObjectCommand.Create(client, endpoint, context, new GetObjectRequest(bucketName, key));
    }
  }
}
