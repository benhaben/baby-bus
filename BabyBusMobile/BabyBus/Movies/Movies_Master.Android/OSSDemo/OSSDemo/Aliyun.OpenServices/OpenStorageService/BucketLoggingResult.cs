// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.BucketLoggingResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// Get Bucket Logging 的请求结果。
  /// 
  /// </summary>
  public class BucketLoggingResult
  {
    /// <summary>
    /// 访问日志记录要存入的bucket。
    /// 
    /// </summary>
    public string TargetBucket { get; internal set; }

    /// <summary>
    /// 存储访问日志记录的object名字前缀，可以为空。
    /// 
    /// </summary>
    public string TargetPrefix { get; internal set; }
  }
}
