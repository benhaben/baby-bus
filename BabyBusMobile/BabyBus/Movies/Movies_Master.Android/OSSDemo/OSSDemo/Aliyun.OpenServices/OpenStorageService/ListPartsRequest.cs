// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.ListPartsRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 指定列出指定Upload ID所属的所有已经上传成功Part的请求参数.
  /// 
  /// </summary>
  public class ListPartsRequest
  {
    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// 获取或者设置<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的值。
    /// 
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// 获取或者设置响应中的最大Part数目
    /// 
    /// </summary>
    public int? MaxParts { get; set; }

    /// <summary>
    /// 获取或者设置List的起始位置，只有Part Number数目大于该参数的Part会被列出。
    /// 
    /// </summary>
    public int? PartNumberMarker { get; set; }

    /// <summary>
    /// 获取或者设置UploadId
    /// 
    /// </summary>
    public string UploadId { get; private set; }

    public ListPartsRequest(string bucketName, string key, string uploadId)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (string.IsNullOrEmpty(uploadId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "uploadId");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      this.BucketName = bucketName;
      this.Key = key;
      this.UploadId = uploadId;
    }
  }
}
