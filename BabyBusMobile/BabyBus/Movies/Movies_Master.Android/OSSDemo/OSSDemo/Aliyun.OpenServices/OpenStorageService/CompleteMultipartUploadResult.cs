// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.CompleteMultipartUploadResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// Complete Multipart Upload的请求结果.
  /// 
  /// </summary>
  public class CompleteMultipartUploadResult
  {
    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; internal set; }

    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Key。
    /// 
    /// </summary>
    public string Key { get; internal set; }

    /// <summary>
    /// 获取新创建Object的URL。
    /// 
    /// </summary>
    public string Location { get; internal set; }

    /// <summary>
    /// 获取新创建的Object的ETag
    /// 
    /// </summary>
    public string ETag { get; internal set; }
  }
}
