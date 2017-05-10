// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.GeneratePresignedUriRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 指定生成URL预签名的请求参数.
  /// 
  /// </summary>
  public class GeneratePresignedUriRequest
  {
    private IDictionary<string, string> _userMetadata = (IDictionary<string, string>) new Dictionary<string, string>();

    /// <summary>
    /// 获取或者设置HttpMethod。
    /// 
    /// </summary>
    public SignHttpMethod Method { get; set; }

    /// <summary>
    /// 获取或者设置Object所在Bucket的名称。
    /// 
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 获取或者设置Object的名称。
    /// 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 获取或者设置过期时间
    /// 
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// 获取或者设置要重载的返回请求头。
    /// 
    /// </summary>
    public ResponseHeaderOverrides ResponseHeaders { get; set; }

    /// <summary>
    /// 获取或者设置用户自定义的元数据，表示以x-oss-meta-为前缀的请求头。
    /// 
    /// </summary>
    public IDictionary<string, string> UserMetadata
    {
      get
      {
        return this._userMetadata;
      }
      set
      {
        this._userMetadata = value == null ? (IDictionary<string, string>) new Dictionary<string, string>() : value;
      }
    }

    public GeneratePresignedUriRequest(string bucketName, string key)
      : this(bucketName, key, SignHttpMethod.Get)
    {
    }

    public GeneratePresignedUriRequest(string bucketName, string key, SignHttpMethod httpMethod)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (key != null && !OssUtils.IsObjectKeyValid(key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      this.BucketName = bucketName;
      this.Key = key;
      this.Method = httpMethod;
    }
  }
}
