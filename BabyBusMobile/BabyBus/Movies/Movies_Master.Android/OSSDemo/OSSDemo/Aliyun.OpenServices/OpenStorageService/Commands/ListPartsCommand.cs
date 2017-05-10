// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.ListPartsCommand
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
  /// Description of ListPartsCommands.
  /// 
  /// </summary>
  internal class ListPartsCommand : OssCommand<PartListing>
  {
    private ListPartsRequest _listPartsRequest;

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Get;
      }
    }

    protected override string Bucket
    {
      get
      {
        return this._listPartsRequest.BucketName;
      }
    }

    protected override string Key
    {
      get
      {
        return this._listPartsRequest.Key;
      }
    }

    protected override IDictionary<string, string> Parameters
    {
      get
      {
        IDictionary<string, string> parameters = base.Parameters;
        ListPartsCommand.Populate(this._listPartsRequest, parameters);
        return parameters;
      }
    }

    private ListPartsCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, PartListing> deserializeMethod, ListPartsRequest listPartsRequest)
      : base(client, endpoint, context, deserializeMethod)
    {
      Debug.Assert(listPartsRequest != null);
      if (string.IsNullOrEmpty(listPartsRequest.BucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(listPartsRequest.Key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (string.IsNullOrEmpty(listPartsRequest.UploadId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");
      if (!OssUtils.IsBucketNameValid(listPartsRequest.BucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(listPartsRequest.Key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      this._listPartsRequest = listPartsRequest;
    }

    private static void Populate(ListPartsRequest listPartsRequst, IDictionary<string, string> parameters)
    {
      parameters["uploadId"] = listPartsRequst.UploadId;
      int? nullable;
      if (listPartsRequst.MaxParts.HasValue)
      {
        IDictionary<string, string> dictionary = parameters;
        string index = "max-parts";
        nullable = listPartsRequst.MaxParts;
        string str = nullable.ToString();
        dictionary[index] = str;
      }
      nullable = listPartsRequst.PartNumberMarker;
      if (!nullable.HasValue)
        return;
      IDictionary<string, string> dictionary1 = parameters;
      string index1 = "part-number-marker";
      nullable = listPartsRequst.PartNumberMarker;
      string str1 = nullable.ToString();
      dictionary1[index1] = str1;
    }

    public static ListPartsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, ListPartsRequest listPartsRequest)
    {
      if (listPartsRequest == null)
        throw new ArgumentNullException("listPartsRequest");
      else
        return new ListPartsCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateListPartsResultDeserializer(), listPartsRequest);
    }
  }
}
