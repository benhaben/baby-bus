// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.ListObjectsRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 包含获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>列表的请求信息。
  /// 
  /// </summary>
  public class ListObjectsRequest
  {
    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// 获取或设置一个值，限定返回的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Key必须以该值作为前缀。
    /// 
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// 获取或设置一个值，用户设定结果从该值之后按字母排序的第一个开始返回。
    /// 
    /// </summary>
    public string Marker { get; set; }

    /// <summary>
    /// 获取或设置一个值，用于限定此次返回object的最大数。
    ///             如果不设定，默认为100。
    /// 
    /// </summary>
    public int? MaxKeys { get; set; }

    /// <summary>
    /// 获取或设置用于对<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>按Key进行分组的字符。
    /// 
    /// </summary>
    public string Delimiter { get; set; }

    /// <summary>
    /// 使用给定的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>名称构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.ListObjectsRequest"/>实体。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    public ListObjectsRequest(string bucketName)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      this.BucketName = bucketName;
    }
  }
}
