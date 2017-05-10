// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.OssObjectSummary
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// <see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的摘要信息。
  /// 
  /// </summary>
  public class OssObjectSummary
  {
    /// <summary>
    /// 获取Object所在的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; internal set; }

    /// <summary>
    /// 获取Object的Key。
    /// 
    /// </summary>
    public string Key { get; internal set; }

    /// <summary>
    /// 获取一个值表示与Object相关的hex编码的128位MD5摘要。
    /// 
    /// </summary>
    public string ETag { get; internal set; }

    /// <summary>
    /// 获取Object的文件字节数。
    /// 
    /// </summary>
    public long Size { get; internal set; }

    /// <summary>
    /// 获取最后修改时间。
    /// 
    /// </summary>
    public DateTime LastModified { get; internal set; }

    /// <summary>
    /// 获取Object的存储类别。
    /// 
    /// </summary>
    public string StorageClass { get; internal set; }

    /// <summary>
    /// 获取Object的<see cref="P:Aliyun.OpenServices.OpenStorageService.OssObjectSummary.Owner"/>。
    /// 
    /// </summary>
    public Owner Owner { get; internal set; }

    /// <summary>
    /// 初始化一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>实例。
    /// 
    /// </summary>
    internal OssObjectSummary()
    {
    }

    /// <summary>
    /// 获取该实例的字符串表示。
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[OSSObjectSummary BucketName={0}, Key={1}]", new object[2]
      {
        (object) this.BucketName,
        (object) this.Key
      });
    }
  }
}
