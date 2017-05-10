// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.ListObjectsCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Transform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of ListObjectsCommand.
  /// 
  /// </summary>
  internal class ListObjectsCommand : OssCommand<ObjectListing>
  {
    private ListObjectsRequest _listObjectsRequest;

    protected override string Bucket
    {
      get
      {
        return this._listObjectsRequest.BucketName;
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        IDictionary<string, string> parameters = base.Parameters;
        ListObjectsCommand.Populate(this._listObjectsRequest, parameters);
        return parameters;
      }
    }

    private ListObjectsCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, ObjectListing> deserializer, ListObjectsRequest listObjectsRequest)
      : base(client, endpoint, context, deserializer)
    {
      Debug.Assert(listObjectsRequest != null);
      this._listObjectsRequest = listObjectsRequest;
    }

    public static ListObjectsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, ListObjectsRequest listObjectsRequest)
    {
      return new ListObjectsCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateListObjectsResultDeserializer(), listObjectsRequest);
    }

    private static void Populate(ListObjectsRequest listObjectsRequest, IDictionary<string, string> parameters)
    {
      if (listObjectsRequest.Prefix != null)
        parameters["prefix"] = listObjectsRequest.Prefix;
      if (listObjectsRequest.Marker != null)
        parameters["marker"] = listObjectsRequest.Marker;
      if (listObjectsRequest.Delimiter != null)
        parameters["delimiter"] = listObjectsRequest.Delimiter;
      if (!listObjectsRequest.MaxKeys.HasValue || !listObjectsRequest.MaxKeys.HasValue)
        return;
      parameters["max-keys"] = listObjectsRequest.MaxKeys.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
