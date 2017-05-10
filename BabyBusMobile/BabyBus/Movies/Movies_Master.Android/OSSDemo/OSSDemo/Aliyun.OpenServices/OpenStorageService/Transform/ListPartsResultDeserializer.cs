// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.ListPartsResponseDeserializer
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
  /// Description of ListPartsResultDeserializer.
  /// 
  /// </summary>
  internal class ListPartsResponseDeserializer : ResponseDeserializer<PartListing, ListPartsResult>
  {
    public ListPartsResponseDeserializer(IDeserializer<Stream, ListPartsResult> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override PartListing Deserialize(ServiceResponse response)
    {
      ListPartsResult listPartsResult = this.ContentDeserializer.Deserialize(response.Content);
      PartListing partListing = new PartListing();
      partListing.BucketName = listPartsResult.Bucket;
      partListing.Key = listPartsResult.Key;
      partListing.MaxParts = listPartsResult.MaxParts;
      partListing.NextPartNumberMarker = listPartsResult.NextPartNumberMarker;
      partListing.PartNumberMarker = listPartsResult.PartNumberMarker;
      partListing.UploadId = listPartsResult.UploadId;
      partListing.IsTruncated = listPartsResult.IsTruncated;
      if (listPartsResult.PartResults != null)
      {
        foreach (ListPartsResult.PartResult partResult in listPartsResult.PartResults)
        {
          Part part1 = new Part();
          Part part2 = part1;
          string str;
          if (partResult.ETag == null)
            str = string.Empty;
          else
            str = partResult.ETag.Trim('"');
          part2.ETag = str;
          part1.LastModified = partResult.LastModified;
          part1.PartNumber = partResult.PartNumber;
          part1.Size = partResult.Size;
          partListing.AddPart(part1);
        }
      }
      return partListing;
    }
  }
}
