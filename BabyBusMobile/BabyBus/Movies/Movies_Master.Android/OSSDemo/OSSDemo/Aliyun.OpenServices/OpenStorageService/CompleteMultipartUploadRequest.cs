// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.CompleteMultipartUploadRequest
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
  /// 指定完成Multipart Upload的请求参数.
  /// 
  /// </summary>
  public class CompleteMultipartUploadRequest
  {
    private IList<PartETag> _partETags = (IList<PartETag>) new List<PartETag>();

    /// <summary>
    /// 获取或者设置<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// 获取或者设置<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的值。
    /// 
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// 获取或设置上传Multipart上传事件的Upload ID。
    /// 
    /// </summary>
    public string UploadId { get; private set; }

    /// <summary>
    /// 获取或者设置标识Part上传结果的<see cref="T:Aliyun.OpenServices.OpenStorageService.PartETag"/>对象列表。
    /// 
    /// </summary>
    public IList<PartETag> PartETags
    {
      get
      {
        return this._partETags;
      }
    }

    public CompleteMultipartUploadRequest(string bucketName, string key, string uploadId)
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
