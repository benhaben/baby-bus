// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Transform.SerializerFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Model;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Transform
{
  /// <summary>
  /// Description of DeserializerFactory.
  /// 
  /// </summary>
  internal abstract class SerializerFactory
  {
    public static SerializerFactory GetFactory()
    {
      return SerializerFactory.GetFactory((string) null);
    }

    public static SerializerFactory GetFactory(string contentType)
    {
      if (contentType == null)
        contentType = "text/xml";
      if (contentType.Contains("xml"))
        return (SerializerFactory) new XmlSerializerFactory();
      else
        return (SerializerFactory) null;
    }

    protected abstract ISerializer<T, Stream> CreateContentSerializer<T>();

    public ISerializer<CompleteMultipartUploadRequest, Stream> CreateCompleteUploadRequestSerializer()
    {
      return (ISerializer<CompleteMultipartUploadRequest, Stream>) new CompleteMultipartUploadRequestSerializer(this.CreateContentSerializer<CompleteMultipartUploadRequestModel>());
    }

    public ISerializer<SetBucketLoggingRequest, Stream> CreateSetBucketLoggingRequestSerializer()
    {
      return (ISerializer<SetBucketLoggingRequest, Stream>) new SetBucketLoggingRequestSerializer(this.CreateContentSerializer<SetBucketLoggingRequestModel>());
    }

    public ISerializer<SetBucketWebsiteRequest, Stream> CreateSetBucketWebsiteRequestSerializer()
    {
      return (ISerializer<SetBucketWebsiteRequest, Stream>) new SetBucketWebsiteRequestSerializer(this.CreateContentSerializer<SetBucketWebsiteRequestModel>());
    }

    public ISerializer<SetBucketCorsRequest, Stream> CreateSetBucketCorsRequestSerializer()
    {
      return (ISerializer<SetBucketCorsRequest, Stream>) new SetBucketCorsRequestSerializer(this.CreateContentSerializer<SetBucketCorsRequestModel>());
    }
  }
}
