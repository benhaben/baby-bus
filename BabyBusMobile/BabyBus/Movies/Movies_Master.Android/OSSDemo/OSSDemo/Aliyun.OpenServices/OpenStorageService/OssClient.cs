// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.OssClient
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Commands;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 访问阿里云开放存储服务（Open Storage Service， OSS）的入口类。
    /// 
    /// </summary>
    public class OssClient : IOss
    {
        private Uri _endpoint;
        private ServiceCredentials _credentials;
        private IServiceClient _serviceClient;

        /// <summary>
        /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssClient"/>实例。
        /// 
        /// </summary>
        /// <param name="accessId">OSS的访问ID。</param><param name="accessKey">OSS的访问密钥。</param>
        public OssClient (string accessId, string accessKey)
            : this (OssUtils.DefaultEndpoint, accessId, accessKey)
        {
        }
        //        UriKind RelativeOrAbsolute
        /// <summary>
        /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssClient"/>实例。
        /// 
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param><param name="accessId">OSS的访问ID。</param><param name="accessKey">OSS的访问密钥。</param>
        public OssClient (string endpoint, string accessId, string accessKey)
            : this (new Uri (endpoint, UriKind.RelativeOrAbsolute), accessId, accessKey)
        {
        }

        /// <summary>
        /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssClient"/>实例。
        /// 
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param><param name="accessId">OSS的访问ID。</param><param name="accessKey">OSS的访问密钥。</param>
        public OssClient (Uri endpoint, string accessId, string accessKey)
            : this (endpoint, accessId, accessKey, new ClientConfiguration ())
        {
        }

        /// <summary>
        /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssClient"/>实例。
        /// 
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param><param name="accessId">OSS的访问ID。</param><param name="accessKey">OSS的访问密钥。</param><param name="configuration">客户端配置。</param>
        public OssClient (Uri endpoint, string accessId, string accessKey, ClientConfiguration configuration)
        {
            if (endpoint == (Uri)null)
                throw new ArgumentNullException ("endpoint");
            if (string.IsNullOrEmpty (accessId))
                throw new ArgumentException (Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
            if (string.IsNullOrEmpty (accessKey))
                throw new ArgumentException (Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");
            //if (!endpoint.ToString ().StartsWith ("http://", StringComparison.OrdinalIgnoreCase))
            //    throw new ArgumentException (OssResources.BucketNameInvalid, "endpoint");
            this._endpoint = endpoint;
            this._credentials = new ServiceCredentials (accessId, accessKey);
            this._serviceClient = ServiceClientFactory.CreateServiceClient (configuration ?? new ClientConfiguration ());
        }

        /// <inheritdoc/>
        public Bucket CreateBucket (string bucketName)
        {
            using (CreateBucketCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, bucketName, (string)null), bucketName).Execute ())
                ;
            return new Bucket (bucketName);
        }

        /// <inheritdoc/>
        public void DeleteBucket (string bucketName)
        {
            using (DeleteBucketCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, bucketName, (string)null), bucketName).Execute ())
                ;
        }

        /// <inheritdoc/>
        public IEnumerable<Bucket> ListBuckets ()
        {
            return ListBucketsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, (string)null, (string)null)).Execute ();
        }

        /// <inheritdoc/>
        public void SetBucketAcl (string bucketName, CannedAccessControlList acl)
        {
            using (SetBucketAclCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, bucketName, (string)null), bucketName, acl).Execute ())
                ;
        }

        /// <inheritdoc/>
        public AccessControlList GetBucketAcl (string bucketName)
        {
            return GetBucketAclCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, bucketName, (string)null), bucketName).Execute ();
        }

        /// <inheritdoc/>
        public void SetBucketCors (SetBucketCorsRequest setBucketCorsRequest)
        {
            using (SetBucketCorsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, setBucketCorsRequest.BucketName, (string)null), setBucketCorsRequest.BucketName, setBucketCorsRequest).Execute ())
                ;
        }

        /// <inheritdoc/>
        public IList<CORSRule> GetBucketCors (string bucketName)
        {
            return GetBucketCorsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, bucketName, (string)null), bucketName).Execute ();
        }

        public void DeleteBucketCors (string bucketName)
        {
            using (DeleteBucketCorsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, bucketName, (string)null), bucketName).Execute ())
                ;
        }

        public void OptionsObject (OptionsRequest optionsRequest)
        {
            using (OptionsObjectCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Options, optionsRequest.BucketName, optionsRequest.Key), optionsRequest.BucketName, optionsRequest).Execute ())
                ;
        }

        /// <inheritdoc/>
        public void SetBucketLogging (SetBucketLoggingRequest setBucketLoggingRequest)
        {
            using (SetBucketLoggingCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, setBucketLoggingRequest.BucketName, (string)null), setBucketLoggingRequest.BucketName, setBucketLoggingRequest).Execute ())
                ;
        }

        /// <inheritdoc/>
        public BucketLoggingResult GetBucketLogging (string bucketName)
        {
            return GetBucketLoggingCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, bucketName, (string)null), bucketName).Execute ();
        }

        /// <inheritdoc/>
        public void DeleteBucketLogging (string bucketName)
        {
            using (DeleteBucketLoggingCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, bucketName, (string)null), bucketName).Execute ())
                ;
        }

        /// <inheritdoc/>
        public void SetBucketWebsite (SetBucketWebsiteRequest setBucketWebSiteRequest)
        {
            using (SetBucketWebsiteCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, setBucketWebSiteRequest.BucketName, (string)null), setBucketWebSiteRequest.BucketName, setBucketWebSiteRequest).Execute ())
                ;
        }

        /// <inheritdoc/>
        public BucketWebsiteResult GetBucketWebsite (string bucketName)
        {
            return GetBucketWebsiteCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, bucketName, (string)null), bucketName).Execute ();
        }

        /// <inheritdoc/>
        public void DeleteBucketWebsite (string bucketName)
        {
            using (DeleteBucketWebsiteCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, bucketName, (string)null), bucketName).Execute ())
                ;
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects (string bucketName)
        {
            return this.ListObjects (bucketName, (string)null);
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects (string bucketName, string prefix)
        {
            return this.ListObjects (new ListObjectsRequest (bucketName) {
                Prefix = prefix
            });
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects (ListObjectsRequest listObjectsRequest)
        {
            if (listObjectsRequest == null)
                throw new ArgumentNullException ("listObjectsRequest");
            else
                return ListObjectsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, listObjectsRequest.BucketName, (string)null), listObjectsRequest).Execute ();
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject (string bucketName, string key, Stream content, ObjectMetadata metadata)
        {
            return PutObjectCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, bucketName, key), bucketName, key, content, metadata).Execute ();
        }

        /// <inheritdoc/>
        public OssObject GetObject (string bucketName, string key)
        {
            return this.GetObject (new GetObjectRequest (bucketName, key));
        }

        /// <inheritdoc/>
        public OssObject GetObject (GetObjectRequest getObjectRequest)
        {
            if (getObjectRequest == null)
                throw new ArgumentNullException ("getObjectRequest");
            else
                return GetObjectCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, getObjectRequest.BucketName, getObjectRequest.Key), getObjectRequest).Execute ();
        }

        /// <inheritdoc/>
        public ObjectMetadata GetObject (GetObjectRequest getObjectRequest, Stream output)
        {
            OssObject @object = this.GetObject (getObjectRequest);
            using (@object.Content)
                IOUtils.WriteTo (@object.Content, output);
            return @object.Metadata;
        }

        /// <inheritdoc/>
        public ObjectMetadata GetObjectMetadata (string bucketName, string key)
        {
            return GetObjectMetadataCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Head, bucketName, key), bucketName, key).Execute ();
        }

        /// <inheritdoc/>
        public void DeleteObject (string bucketName, string key)
        {
            using (DeleteObjectCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, bucketName, key), bucketName, key).Execute ())
                ;
        }

        /// <inheritdoc/>
        public CopyObjectResult CopyObject (CopyObjectRequest copyObjectRequst)
        {
            return CopyObjectCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, copyObjectRequst.DestinationBucketName, copyObjectRequst.DestinationKey), copyObjectRequst).Execute ();
        }

        /// <inheritdoc/>
        public Uri GeneratePresignedUri (string bucketName, string key, DateTime expiration)
        {
            return this.GeneratePresignedUri (new GeneratePresignedUriRequest (bucketName, key, SignHttpMethod.Get) {
                Expiration = expiration
            });
        }

        /// <inheritdoc/>
        public Uri GeneratePresignedUri (string bucketName, string key, DateTime expiration, SignHttpMethod method)
        {
            return this.GeneratePresignedUri (new GeneratePresignedUriRequest (bucketName, key, method) {
                Expiration = expiration
            });
        }

        /// <inheritdoc/>
        public Uri GeneratePresignedUri (GeneratePresignedUriRequest generatePresignedUriRequest)
        {
            if (generatePresignedUriRequest == null)
                throw new ArgumentNullException ("generatePresignedUriRequest");
            DateTime expiration = generatePresignedUriRequest.Expiration;
            bool flag = 0 == 0;
            string accessId = this._credentials.AccessId;
            string accessKey = this._credentials.AccessKey;
            string bucketName = generatePresignedUriRequest.BucketName;
            string key = generatePresignedUriRequest.Key;
            long num = 621355968000000000L;
            DateTime dateTime = generatePresignedUriRequest.Expiration;
            dateTime = dateTime.ToUniversalTime ();
            string str1 = ((dateTime.Ticks - num) / 10000000L).ToString ();
            string str2 = OssUtils.MakeResourcePath (key);
            ServiceRequest request = new ServiceRequest ();
            request.Endpoint = OssUtils.MakeBucketEndpoint (this._endpoint, bucketName);
            request.ResourcePath = str2;
            if (generatePresignedUriRequest.Method == SignHttpMethod.Get)
                request.Method = HttpMethod.Get;
            else if (generatePresignedUriRequest.Method == SignHttpMethod.Put)
                request.Method = HttpMethod.Put;
            else if (generatePresignedUriRequest.Method == SignHttpMethod.Post)
                request.Method = HttpMethod.Post;
            else if (generatePresignedUriRequest.Method == SignHttpMethod.Head)
                request.Method = HttpMethod.Head;
            else if (generatePresignedUriRequest.Method == SignHttpMethod.Delete)
                request.Method = HttpMethod.Delete;
            request.Headers.Add ("Date", str1);
            foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) generatePresignedUriRequest.UserMetadata)
                request.Headers.Add ("x-oss-meta-" + keyValuePair.Key, keyValuePair.Value);
            if (generatePresignedUriRequest.ResponseHeaders != null)
                generatePresignedUriRequest.ResponseHeaders.Populate (request.Parameters);
            string resourcePath = "/" + (bucketName ?? "") + (key != null ? "/" + key : "");
            string data = SignUtils.BuildCanonicalString (((object)generatePresignedUriRequest.Method).ToString ().ToUpperInvariant (), resourcePath, request);
            string signature = ServiceSignature.Create ().ComputeSignature (accessKey, data);
            request.Parameters.Add ("Expires", str1);
            request.Parameters.Add ("OSSAccessKeyId", accessId);
            request.Parameters.Add ("Signature", signature);
            string requestParameterString = HttpUtils.GetRequestParameterString ((IEnumerable<KeyValuePair<string, string>>)request.Parameters);
            string str3 = request.Endpoint.ToString ();
            if (!str3.EndsWith ("/"))
                str3 = str3 + "/";
            return new Uri (str3 + str2 + "?" + requestParameterString);
        }

        /// <inheritdoc/>
        public MultipartUploadListing ListMultipartUploads (ListMultipartUploadsRequest listMultipartUploadsRequest)
        {
            return ListMultipartUploadsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, listMultipartUploadsRequest.BucketName, (string)null), listMultipartUploadsRequest).Execute ();
        }

        /// <inheritdoc/>
        public InitiateMultipartUploadResult InitiateMultipartUpload (InitiateMultipartUploadRequest initiateMultipartUploadRequest)
        {
            return InitiateMultipartUploadCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Post, initiateMultipartUploadRequest.BucketName, initiateMultipartUploadRequest.Key), initiateMultipartUploadRequest).Execute ();
        }

        /// <inheritdoc/>
        public void AbortMultipartUpload (AbortMultipartUploadRequest abortMultipartUploadRequest)
        {
            using (AbortMultipartUploadCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Delete, abortMultipartUploadRequest.BucketName, abortMultipartUploadRequest.Key), abortMultipartUploadRequest).Execute ())
                ;
        }

        /// <inheritdoc/>
        public UploadPartResult UploadPart (UploadPartRequest uploadPartRequest)
        {
            return UploadPartCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Put, uploadPartRequest.BucketName, uploadPartRequest.Key), uploadPartRequest).Execute ();
        }

        /// <inheritdoc/>
        public PartListing ListParts (ListPartsRequest listPartsRequest)
        {
            return ListPartsCommand.Create (this.GetServiceClient (), this._endpoint, this.CreateContext (HttpMethod.Get, listPartsRequest.BucketName, listPartsRequest.Key), listPartsRequest).Execute ();
        }

        /// <inheritdoc/>
        public CompleteMultipartUploadResult CompleteMultipartUpload (CompleteMultipartUploadRequest completeMultipartUploadRequest)
        {
            return CompleteMultipartUploadCommand.Create (this.GetServiceClient (),
                this._endpoint
                , this.CreateContext (HttpMethod.Post, completeMultipartUploadRequest.BucketName, completeMultipartUploadRequest.Key)
                , completeMultipartUploadRequest).Execute ();
        }

        private IServiceClient GetServiceClient ()
        {
            return this._serviceClient;
        }

        private ExecutionContext CreateContext (HttpMethod method, string bucket, string key)
        {
            ExecutionContextBuilder executionContextBuilder = new ExecutionContextBuilder ();
            executionContextBuilder.Bucket = bucket;
            executionContextBuilder.Key = key;
            executionContextBuilder.Method = method;
            executionContextBuilder.Credentials = this._credentials;
            executionContextBuilder.ResponseHandlers.Add ((IResponseHandler)new ErrorResponseHandler ());
            return executionContextBuilder.Build ();
        }
    }
}
