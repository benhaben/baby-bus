// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.ListObjectsResponseDeserializer
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of ListObjectsResponseDeserializer.
  /// 
  /// </summary>
  internal class ListObjectsResponseDeserializer : ResponseDeserializer<ObjectListing, ListBucketResult>
  {
    public ListObjectsResponseDeserializer(IDeserializer<Stream, ListBucketResult> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override ObjectListing Deserialize(ServiceResponse response)
    {
      ListBucketResult listBucketResult = this.ContentDeserializer.Deserialize(response.Content);
      ObjectListing objectListing = new ObjectListing(listBucketResult.Name);
      objectListing.Delimiter = listBucketResult.Delimiter;
      objectListing.IsTrunked = listBucketResult.IsTruncated;
      objectListing.Marker = listBucketResult.Marker;
      objectListing.MaxKeys = listBucketResult.MaxKeys;
      objectListing.NextMarker = listBucketResult.NextMarker;
      objectListing.Prefix = listBucketResult.Prefix;
      if (listBucketResult.Contents != null)
      {
        foreach (ListBucketResultContents bucketResultContents in listBucketResult.Contents)
        {
          OssObjectSummary summary = new OssObjectSummary();
          summary.BucketName = listBucketResult.Name;
          summary.Key = bucketResultContents.Key;
          summary.LastModified = bucketResultContents.LastModified;
          OssObjectSummary ossObjectSummary = summary;
          string str;
          if (bucketResultContents.ETag == null)
            str = string.Empty;
          else
            str = bucketResultContents.ETag.Trim('"');
          ossObjectSummary.ETag = str;
          summary.Size = bucketResultContents.Size;
          summary.StorageClass = bucketResultContents.StorageClass;
          summary.Owner = bucketResultContents.Owner != null ? new Owner(bucketResultContents.Owner.Id, bucketResultContents.Owner.DisplayName) : (Owner) null;
          objectListing.AddObjectSummary(summary);
        }
      }
      if (listBucketResult.CommonPrefixes != null)
      {
        foreach (ListBucketResultCommonPrefixes resultCommonPrefixes in listBucketResult.CommonPrefixes)
        {
          if (resultCommonPrefixes.Prefix != null)
          {
            foreach (string prefix in resultCommonPrefixes.Prefix)
              objectListing.AddCommonPrefix(prefix);
          }
        }
      }
      return objectListing;
    }
  }
}
