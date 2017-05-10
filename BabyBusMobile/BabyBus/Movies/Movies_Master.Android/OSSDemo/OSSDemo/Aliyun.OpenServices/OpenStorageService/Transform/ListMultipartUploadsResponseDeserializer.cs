// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.ListMultipartUploadsResponseDeserializer
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
  /// Description of ListMultipartUploadsResponseDeserializer.
  /// 
  /// </summary>
  internal class ListMultipartUploadsResponseDeserializer : ResponseDeserializer<MultipartUploadListing, ListMultipartUploadsResult>
  {
    public ListMultipartUploadsResponseDeserializer(IDeserializer<Stream, ListMultipartUploadsResult> contentDeserializer)
      : base(contentDeserializer)
    {
    }

    public override MultipartUploadListing Deserialize(ServiceResponse response)
    {
      ListMultipartUploadsResult multipartUploadsResult = this.ContentDeserializer.Deserialize(response.Content);
      MultipartUploadListing multipartUploadListing = new MultipartUploadListing(multipartUploadsResult.Bucket);
      multipartUploadListing.BucketName = multipartUploadsResult.Bucket;
      multipartUploadListing.Delimiter = multipartUploadsResult.Delimiter;
      multipartUploadListing.IsTruncated = multipartUploadsResult.IsTruncated;
      multipartUploadListing.KeyMarker = multipartUploadsResult.KeyMarker;
      multipartUploadListing.MaxUploads = multipartUploadsResult.MaxUploads;
      multipartUploadListing.NextKeyMarker = multipartUploadsResult.NextKeyMarker;
      multipartUploadListing.NextUploadIdMarker = multipartUploadsResult.NextUploadIdMarker;
      multipartUploadListing.Prefix = multipartUploadsResult.Prefix;
      multipartUploadListing.UploadIdMarker = multipartUploadsResult.UploadIdMarker;
      if (multipartUploadsResult.CommonPrefix != null && multipartUploadsResult.CommonPrefix.Prefixs != null)
      {
        foreach (string prefix in multipartUploadsResult.CommonPrefix.Prefixs)
          multipartUploadListing.AddCommonPrefix(prefix);
      }
      if (multipartUploadsResult.Uploads != null)
      {
        foreach (ListMultipartUploadsResult.Upload upload in multipartUploadsResult.Uploads)
          multipartUploadListing.AddMultipartUpload(new MultipartUpload()
          {
            Initiated = upload.Initiated,
            Key = upload.Key,
            UploadId = upload.UploadId,
            StorageClass = upload.StorageClass
          });
      }
      return multipartUploadListing;
    }
  }
}
