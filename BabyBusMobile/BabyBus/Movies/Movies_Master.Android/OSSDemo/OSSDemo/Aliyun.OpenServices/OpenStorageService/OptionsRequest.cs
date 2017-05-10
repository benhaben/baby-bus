// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.OptionsRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService
{
  public class OptionsRequest
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
    /// 请求来源域，用来标示跨域请求。
    /// 
    /// </summary>
    public string Origin { get; set; }

    /// <summary>
    /// 表示在实际请求中将会用到的方法。
    /// 
    /// </summary>
    public string AccessControlRequestMethod { get; set; }

    /// <summary>
    /// 表示在实际请求中会用到的除了简单头部之外的headers。
    /// 
    /// </summary>
    public string AccessControlRequestHeaders { get; set; }

    public OptionsRequest(string bucketName, string key)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      this.BucketName = bucketName;
      this.Key = key;
    }
  }
}
