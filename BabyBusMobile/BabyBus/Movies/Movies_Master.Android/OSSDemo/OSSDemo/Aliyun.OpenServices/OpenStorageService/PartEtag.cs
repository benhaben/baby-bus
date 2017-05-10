// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.PartETag
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 某块PartNumber和ETag的信息，用于Complete Multipart Upload请求参数的设置。
  /// 
  /// </summary>
  public class PartETag
  {
    /// <summary>
    /// 获取或者设置一个值表示表示分块的标识
    /// 
    /// </summary>
    public int PartNumber { get; set; }

    /// <summary>
    /// 获取或者设置一个值表示与Object相关的hex编码的128位MD5摘要。
    /// 
    /// </summary>
    public string ETag { get; set; }

    public PartETag(int partNumber, string eTag)
    {
      this.PartNumber = partNumber;
      this.ETag = eTag;
    }
  }
}
