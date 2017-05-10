// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.OssErrorCode
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 表示来自开放存储服务（Open Storage Service，OSS）的错误代码。
  /// 
  /// </summary>
  /// <seealso cref="P:OssException.ErrorCode"/>。
  public static class OssErrorCode
  {
    /// <summary>
    /// 访问被拒绝。
    /// 
    /// </summary>
    public const string AccessDenied = "AccessDenied";
    /// <summary>
    /// Bucket已经存在。
    /// 
    /// </summary>
    public const string BucketAlreadyExists = "BucketAlreadyExists";
    /// <summary>
    /// Bucket不为空。
    /// 
    /// </summary>
    public const string BucketNotEmtpy = "BucketNotEmtpy";
    /// <summary>
    /// 实体过大。
    /// 
    /// </summary>
    public const string EntityTooLarge = "EntityTooLarge";
    /// <summary>
    /// 实体过小。
    /// 
    /// </summary>
    public const string EntityTooSmall = "EntityTooSmall";
    /// <summary>
    /// 参数格式错误。
    /// 
    /// </summary>
    public const string InvalidArgument = "InvalidArgument";
    /// <summary>
    /// Access ID不存在
    /// 
    /// </summary>
    public const string InvalidAccessKeyId = "InvalidAccessKeyId";
    /// <summary>
    /// 无效的Bucket名字。
    /// 
    /// </summary>
    public const string InvalidBucketName = "InvalidBucketName";
    /// <summary>
    /// 无效的摘要。
    /// 
    /// </summary>
    public const string InvalidDigest = "InvalidDigest";
    /// <summary>
    /// 无效的Object名字。
    /// 
    /// </summary>
    public const string InvalidObjectName = "InvalidObjectName";
    /// <summary>
    /// OSS内部错误。
    /// 
    /// </summary>
    public const string InternalError = "InternalError";
    /// <summary>
    /// Bucket不存在。
    /// 
    /// </summary>
    public const string NoSuchBucket = "NoSuchBucket";
    /// <summary>
    /// 文件不存在。
    /// 
    /// </summary>
    public const string NoSuchKey = "NoSuchKey";
    /// <summary>
    /// 发起请求的时间和服务器时间超出15分钟。
    /// 
    /// </summary>
    public const string RequestTimeTooSkewed = "RequestTimeTooSkewed";
    /// <summary>
    /// 请求超时。
    /// 
    /// </summary>
    public const string RequestTimeout = "RequestTimeout";
    /// <summary>
    /// 签名错误。
    /// 
    /// </summary>
    public const string SignatureDoesNotMatch = "SignatureDoesNotMatch";
    /// <summary>
    /// 用户的Bucket数目超过限制。
    /// 
    /// </summary>
    public const string TooManyBuckets = "TooManyBuckets";
    /// <summary>
    /// 源Bucket未设置静态网站托管功能
    /// 
    /// </summary>
    public const string NoSuchWebsiteConfiguration = "NoSuchWebsiteConfiguration";
    /// <summary>
    /// CORS规则不存在
    /// 
    /// </summary>
    public const string NoSuchCORSConfiguration = "NoSuchCORSConfiguration";
  }
}
