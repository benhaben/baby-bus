// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.ListBucketResultDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of ListBucketResultDeserializer.
  /// 
  /// </summary>
  internal class ListBucketResultDeserializer : ResponseDeserializer<IEnumerable<Bucket>, ListAllMyBucketsResult>
  {
    public ListBucketResultDeserializer(IDeserializer<Stream, ListAllMyBucketsResult> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override IEnumerable<Bucket> Deserialize(ServiceResponse response)
    {
      ListAllMyBucketsResult model = this.ContentDeserializer.Deserialize(response.Content);
      return Enumerable.Select<BucketModel, Bucket>((IEnumerable<BucketModel>) model.Buckets, (Func<BucketModel, Bucket>) (e => new Bucket(e.Name)
      {
        Owner = new Owner(model.Owner.Id, model.Owner.DisplayName),
        CreationDate = e.CreationDate
      }));
    }
  }
}
