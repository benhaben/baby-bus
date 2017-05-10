// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.IOss
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 阿里云开放存储服务（Open Storage Service， OSS）的访问接口。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// 阿里云存储服务（Open Storage Service，简称OSS），是阿里云对外提供的海量，安全，低成本，
  ///             高可靠的云存储服务。用户可以通过简单的REST接口，在任何时间、任何地点上传和下载数据，
  ///             也可以使用WEB页面对数据进行管理。
  ///             基于OSS，用户可以搭建出各种多媒体分享网站、网盘、个人企业数据备份等基于大规模数据的服务。
  /// 
  /// </para>
  /// 
  /// <para>
  /// OSS的访问地址： http://oss.aliyuncs.com
  /// 
  /// </para>
  /// 
  /// <para>
  /// OSS的web体验地址：http://oss.aliyun.com/
  /// 
  /// </para>
  /// 
  /// </remarks>
  public interface IOss
  {
    /// <summary>
    /// 在OSS中创建一个新的Bucket。
    /// 
    /// </summary>
    /// <param name="bucketName">要创建的Bucket的名称。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>对象。
    /// </returns>
    /// 
    /// <example>
    /// 下面的示例代码演示了如何在OSS中创建一个Bucket，以及如何在进行OSS操作时处理特定的异常消息。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using Aliyun.OpenServices.OpenStorageService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenStorageService
    /// {
    ///     public static class CreateBucketSample
    ///     {
    ///         public static void CreateBucket()
    ///         {
    ///             string accessId = "<your access id>";
    ///             string accessKey = "<your access key>";
    ///             string bucketName = "com_aliyun_sdk_java_sample_bucket1";
    /// 
    ///             OssClient ossClient = new OssClient(endpoint, accessId, accessKey);
    ///             try
    ///             {
    ///                 ossClient.CreateBucket(bucketName);
    ///                 Console.WriteLine("创建成功！Bucket: " + bucketName);
    ///             }
    ///             catch(OssException ex)
    ///             {
    ///                 if (ex.ErrorCode == OssErrorCode.BucketAlreadyExists)
    ///                 {
    ///                     // 这里示例处理一种特定的ErrorCode。
    ///                     Console.WriteLine(string.Format("Bucket '{0}' 已经存在，请更改名称后再创建。", bucketName));
    ///                 }
    ///                 else
    ///                 {
    ///                     // RequestID和HostID可以在有问题时用于联系客服诊断异常。
    ///                     Console.WriteLine(string.Format("创建失败。错误代码：{0}; 错误消息：{1}。\nRequestID:{2}\tHostID:{3}",
    ///                                                     ex.ErrorCode,
    ///                                                     ex.Message,
    ///                                                     ex.RequestId,
    ///                                                     ex.HostId));
    ///                 }
    ///             }
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// 
    /// </example>
    Bucket CreateBucket(string bucketName);

    /// <summary>
    /// 在OSS中删除一个Bucket。
    /// 
    /// </summary>
    /// <param name="bucketName">要删除的Bucket的名称。</param>
    void DeleteBucket(string bucketName);

    /// <summary>
    /// 返回请求者拥有的所有<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的列表。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 请求者拥有的所有<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的列表。
    /// </returns>
    IEnumerable<Bucket> ListBuckets();

    /// <summary>
    /// 设置指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的访问控制列表<see cref="T:Aliyun.OpenServices.OpenStorageService.AccessControlList"/>。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param><param name="acl"><see cref="T:Aliyun.OpenServices.OpenStorageService.CannedAccessControlList"/>枚举中的访问控制列表。</param>
    /// <example>
    /// 下面的示例代码演示了如何将一个Bucket的设置为所有人可以读取。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using Aliyun.OpenServices.OpenStorageService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenStorageService
    /// {
    ///     public static class SetBucketAclSample
    ///     {
    ///         public static void SetBucketAcl()
    ///         {
    ///             string accessId = "<your access id>";
    ///             string accessKey = "<your access key>";
    ///             string bucketName = "com_aliyun_sdk_java_sample_bucket1";
    /// 
    ///             OssClient ossClient = new OssClient(accessId, accessKey);
    ///             // 设置Bucket的ACL为所有人可读。
    ///             ossClient.SetBucketAcl(bucketName, CannedAccessControlList.PublicRead);
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// 
    /// </example>
    void SetBucketAcl(string bucketName, CannedAccessControlList acl);

    /// <summary>
    /// 获取指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的访问控制列表<see cref="T:Aliyun.OpenServices.OpenStorageService.AccessControlList"/>。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    /// <returns>
    /// 访问控制列表<see cref="T:Aliyun.OpenServices.OpenStorageService.AccessControlList"/>的实例。
    /// </returns>
    AccessControlList GetBucketAcl(string bucketName);

    /// <summary>
    /// 设置指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的跨域资源共享(CORS)的规则，如果原规则存在则覆盖原规则。
    /// 
    /// </summary>
    /// <param name="setBucketCorsRequest"/>
    void SetBucketCors(SetBucketCorsRequest setBucketCorsRequest);

    /// <summary>
    /// 获取指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的CORS规则。
    /// 
    /// </summary>
    /// <param name="bucketName"/>
    /// <returns/>
    IList<CORSRule> GetBucketCors(string bucketName);

    /// <summary>
    /// 关闭指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>对应的CORS功能并清空所有规则。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    void DeleteBucketCors(string bucketName);

    /// <summary>
    /// 浏览器在发送跨域请求之前会发送一个preflight请求（OPTIONS）并带上特定的来源域，
    ///             HTTP方法和header信息等给OSS以决定是否发送真正的请求。
    ///             OSS可以通过Put Bucket cors接口来开启<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的CORS支持，开启CORS功能之后，
    ///             OSS在收到浏览器preflight请求时会根据设定的规则评估是否允许本次请求。
    ///             如果不允许或者CORS功能没有开启，返回403 Forbidden。
    /// 
    /// </summary>
    /// <param name="optionsRequest"/>
    void OptionsObject(OptionsRequest optionsRequest);

    /// <summary>
    /// 设置<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的访问日志记录功能。
    ///             这个功能开启后，OSS将自动记录访问这个<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>请求的详细信息，并按照用户指定的规则，
    ///             以小时为单位，将访问日志作为一个Object写入用户指定的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>。
    /// 
    /// </summary>
    /// <param name="setBucketLoggingRequest"/>
    void SetBucketLogging(SetBucketLoggingRequest setBucketLoggingRequest);

    /// <summary>
    /// 查看<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的访问日志配置。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    /// <returns/>
    BucketLoggingResult GetBucketLogging(string bucketName);

    /// <summary>
    /// 关闭<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的访问日志记录功能。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    void DeleteBucketLogging(string bucketName);

    /// <summary>
    /// 将一个<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>设置成静态网站托管模式。
    /// 
    /// </summary>
    /// <param name="setBucketWebSiteRequest"/>
    void SetBucketWebsite(SetBucketWebsiteRequest setBucketWebSiteRequest);

    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的静态网站托管状态。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    /// <returns/>
    BucketWebsiteResult GetBucketWebsite(string bucketName);

    /// <summary>
    /// 关闭<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的静态网站托管模式。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    void DeleteBucketWebsite(string bucketName);

    /// <summary>
    /// 列出指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>下<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的摘要信息<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的列表信息。
    /// </returns>
    ObjectListing ListObjects(string bucketName);

    /// <summary>
    /// 列出指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>下其Key以prefix为前缀<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>
    ///             的摘要信息<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param><param name="prefix">限定返回的<see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Key"/>必须以此作为前缀。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的列表信息。
    /// </returns>
    ObjectListing ListObjects(string bucketName, string prefix);

    /// <summary>
    /// 列出指定<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>下<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的摘要信息<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>。
    /// 
    /// </summary>
    /// <param name="listObjectsRequest">请求参数。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的列表信息。
    /// </returns>
    ObjectListing ListObjects(ListObjectsRequest listObjectsRequest);

    /// <summary>
    /// 上传指定的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>到指定的OSS<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>。
    /// 
    /// </summary>
    /// <param name="bucketName">指定的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>名称。</param><param name="key"><see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的<see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Key"/>。</param><param name="content"><see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的<see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Content"/>。</param><param name="metadata"><see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的元信息。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.PutObjectResult"/>实例。
    /// </returns>
    /// 
    /// <example>
    /// 下面的示例代码演示了如何将一个Bucket的设置为所有人可以读取。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using Aliyun.OpenServices.OpenStorageService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenStorageService
    /// {
    ///     public static class PutObjectSample
    ///     {
    ///         public static void PutObject()
    ///         {
    ///             string accessId = "<your access id>";
    ///             string accessKey = "<your access key>";
    ///             string bucketName = "com_aliyun_sdk_java_sample_bucket1";
    ///             string key = "sampleobject";
    ///             string fileToUpload = "file.zip";
    /// 
    ///             ObjectMetadata metadata = new ObjectMetadata();
    ///             // 可以设定自定义的metadata。
    ///             metadata.UserMetadata.Add("myfield", "test");
    /// 
    ///             OssClient ossClient = new OssClient(accessId, accessKey);
    ///             using(var fs = File.Open(fileToUpload, FileMode.Open))
    ///             {
    ///                 ossClient.PutObject(bucketName, key, fs, metadata);
    ///             }
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// 
    /// </example>
    PutObjectResult PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata);

    /// <summary>
    /// 从指定的OSS<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>中获取指定的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>。
    /// 
    /// </summary>
    /// <param name="bucketName">要获取的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param><param name="key">要获取的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的<see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Key"/>。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>实例。
    /// </returns>
    OssObject GetObject(string bucketName, string key);

    /// <summary>
    /// 从指定的OSS<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>中获取满足请求参数<see cref="T:Aliyun.OpenServices.OpenStorageService.GetObjectRequest"/>的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>。
    /// 
    /// </summary>
    /// <param name="getObjectRequest">请求参数。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>实例。使用后需要释放此对象以释放HTTP连接。
    /// </returns>
    OssObject GetObject(GetObjectRequest getObjectRequest);

    /// <summary>
    /// 从指定的OSS<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>中获取指定的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>，
    ///             并导出到指定的输出流。
    /// 
    /// </summary>
    /// <param name="getObjectRequest">请求参数。</param><param name="output">输出流。</param>
    /// <returns>
    /// 导出<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的元信息。
    /// </returns>
    ObjectMetadata GetObject(GetObjectRequest getObjectRequest, Stream output);

    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的元信息。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param><param name="key"><see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Key"/>。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的元信息。
    /// </returns>
    ObjectMetadata GetObjectMetadata(string bucketName, string key);

    /// <summary>
    /// 删除指定的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param><param name="key"><see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Key"/>。</param>
    void DeleteObject(string bucketName, string key);

    /// <summary>
    /// 复制一个Object
    /// 
    /// </summary>
    /// <param name="copyObjectRequst">请求参数</param>
    /// <returns>
    /// 返回的结果
    /// </returns>
    CopyObjectResult CopyObject(CopyObjectRequest copyObjectRequst);

    /// <summary>
    /// 生成一个签名的Uri。
    /// 
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>
    /// 访问<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Uri。
    /// </returns>
    Uri GeneratePresignedUri(GeneratePresignedUriRequest request);

    /// <summary>
    /// 生成一个用HTTP GET方法访问<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Uri。
    /// 
    /// </summary>
    /// <param name="bucketName">Bucket名称。</param><param name="key">Object的Key</param><param name="expiration">Uri的超时时间。</param>
    /// <returns>
    /// 访问<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Uri。
    /// </returns>
    Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration);

    /// <summary>
    /// 生成一个用指定方法访问<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Uri。
    /// 
    /// </summary>
    /// <param name="bucketName">Bucket名称。</param><param name="key">Object的Key</param><param name="expiration">Uri的超时时间。</param><param name="method">访问Uri的方法</param>
    /// <returns>
    /// 访问<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Uri。
    /// </returns>
    Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration, SignHttpMethod method);

    /// <summary>
    /// 列出所有执行中的Multipart Upload事件
    /// 
    /// </summary>
    /// <param name="listMultipartUploadsRequest">请求参数</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.MultipartUploadListing"/>的列表信息。
    /// </returns>
    MultipartUploadListing ListMultipartUploads(ListMultipartUploadsRequest listMultipartUploadsRequest);

    /// <summary>
    /// 初始化一个Multipart Upload事件
    /// 
    /// </summary>
    /// <param name="initiateMultipartUploadRequest">请求参数</param>
    /// <returns>
    /// 初始化结果
    /// </returns>
    InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest initiateMultipartUploadRequest);

    /// <summary>
    /// 中止一个Multipart Upload事件
    /// 
    /// </summary>
    /// <param name="initiateMultipartUploadRequest">请求参数</param>
    void AbortMultipartUpload(AbortMultipartUploadRequest abortMultipartUploadRequest);

    /// <summary>
    /// 上传某分块的数据
    /// 
    /// </summary>
    /// <param name="uploadPartRequest">请求参数</param>
    /// <returns>
    /// 分块上传结果
    /// </returns>
    UploadPartResult UploadPart(UploadPartRequest uploadPartRequest);

    /// <summary>
    /// 列出已经上传成功的Part
    /// 
    /// </summary>
    /// <param name="listPartsRequest">请求参数</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.PartListing"/>的列表信息。
    /// </returns>
    PartListing ListParts(ListPartsRequest listPartsRequest);

    /// <summary>
    /// 完成分块上传
    /// 
    /// </summary>
    /// <param name="listPartsRequest">请求参数</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenStorageService.CompleteMultipartUploadResult"/>的列表信息。
    /// </returns>
    CompleteMultipartUploadResult CompleteMultipartUpload(CompleteMultipartUploadRequest completeMultipartUploadRequest);
  }
}
