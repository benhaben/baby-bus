// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.SetBucketLoggingRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  public class SetBucketLoggingRequest
  {
    /// <summary>
    /// 获取或者设置<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// 获取或设置存放访问日志的Bucket。
    /// 
    /// </summary>
    public string TargetBucket { get; private set; }

    /// <summary>
    /// 获取或设置存放访问日志的文件名前缀。
    /// 
    /// </summary>
    public string TargetPrefix { get; private set; }

    public SetBucketLoggingRequest(string bucketName, string targetBucket, string targetPrefix)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      this.BucketName = bucketName;
      this.TargetBucket = targetBucket;
      this.TargetPrefix = targetPrefix;
    }
  }
}
