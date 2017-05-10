// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.DeserializerFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of DeserializerFactory.
  /// 
  /// </summary>
  internal abstract class DeserializerFactory
  {
    public static DeserializerFactory GetFactory()
    {
      return DeserializerFactory.GetFactory((string) null);
    }

    public static DeserializerFactory GetFactory(string contentType)
    {
      if (contentType == null)
        contentType = "text/xml";
      if (contentType.Contains("xml"))
        return (DeserializerFactory) new XmlDeserializerFactory();
      else
        return (DeserializerFactory) null;
    }

    protected abstract IDeserializer<Stream, T> CreateContentDeserializer<T>();

    public IDeserializer<ServiceResponse, ErrorResult> CreateErrorResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, ErrorResult>) new SimpleResponseDeserializer<ErrorResult>(this.CreateContentDeserializer<ErrorResult>());
    }

    public IDeserializer<ServiceResponse, IEnumerable<Bucket>> CreateListBucketResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, IEnumerable<Bucket>>) new ListBucketResultDeserializer(this.CreateContentDeserializer<ListAllMyBucketsResult>());
    }

    public IDeserializer<ServiceResponse, AccessControlList> CreateGetAclResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, AccessControlList>) new GetAclResponseDeserializer(this.CreateContentDeserializer<AccessControlPolicy>());
    }

    public IDeserializer<ServiceResponse, IList<CORSRule>> CreateGetCorsResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, IList<CORSRule>>) new GetCorsResponseDeserializer(this.CreateContentDeserializer<SetBucketCorsRequestModel>());
    }

    public IDeserializer<ServiceResponse, BucketLoggingResult> CreateGetBucketLoggingResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, BucketLoggingResult>) new GetBucketLoggingResultDeserializer(this.CreateContentDeserializer<SetBucketLoggingRequestModel>());
    }

    public IDeserializer<ServiceResponse, BucketWebsiteResult> CreateGetBucketWebSiteResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, BucketWebsiteResult>) new GetBucketWebSiteResultDeserializer(this.CreateContentDeserializer<SetBucketWebsiteRequestModel>());
    }

    public IDeserializer<ServiceResponse, PutObjectResult> CreatePutObjectReusltDeserializer()
    {
      return (IDeserializer<ServiceResponse, PutObjectResult>) new PutObjectResponseDeserializer();
    }

    public IDeserializer<ServiceResponse, OssObject> CreateGetObjectResultDeserializer(GetObjectRequest request)
    {
      return (IDeserializer<ServiceResponse, OssObject>) new GetObjectResponseDeserializer(request);
    }

    public IDeserializer<ServiceResponse, ObjectMetadata> CreateGetObjectMetadataResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, ObjectMetadata>) new GetObjectMetadataResponseDeserializer();
    }

    public IDeserializer<ServiceResponse, ObjectListing> CreateListObjectsResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, ObjectListing>) new ListObjectsResponseDeserializer(this.CreateContentDeserializer<ListBucketResult>());
    }

    public IDeserializer<ServiceResponse, MultipartUploadListing> CreateListMultipartUploadsResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, MultipartUploadListing>) new ListMultipartUploadsResponseDeserializer(this.CreateContentDeserializer<ListMultipartUploadsResult>());
    }

    public IDeserializer<ServiceResponse, InitiateMultipartUploadResult> CreateInitiateMultipartUploadResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, InitiateMultipartUploadResult>) new InitiateMultipartUploadResultDeserializer(this.CreateContentDeserializer<InitiateMultipartResult>());
    }

    public IDeserializer<ServiceResponse, UploadPartResult> CreateUploadPartResultDeserializer(int partNumber)
    {
      return (IDeserializer<ServiceResponse, UploadPartResult>) new UploadPartResultDeserializer(partNumber);
    }

    public IDeserializer<ServiceResponse, PartListing> CreateListPartsResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, PartListing>) new ListPartsResponseDeserializer(this.CreateContentDeserializer<ListPartsResult>());
    }

    public IDeserializer<ServiceResponse, CompleteMultipartUploadResult> CreateCompleteUploadResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, CompleteMultipartUploadResult>) new CompleteMultipartUploadResultDeserializer(this.CreateContentDeserializer<CompleteMultipartUploadResultModel>());
    }

    public IDeserializer<ServiceResponse, CopyObjectResult> CreateCopyObjectResultDeserializer()
    {
      return (IDeserializer<ServiceResponse, CopyObjectResult>) new CopyObjectResultDeserializer(this.CreateContentDeserializer<CopyObjectResultModel>());
    }
  }
}
