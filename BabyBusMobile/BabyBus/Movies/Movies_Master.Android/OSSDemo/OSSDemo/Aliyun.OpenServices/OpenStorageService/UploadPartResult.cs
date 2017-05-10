// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.UploadPartResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 获取上传某分块的结果.
  /// 
  /// </summary>
  public class UploadPartResult
  {
    /// <summary>
    /// 获取一个值表示与Object相关的hex编码的128位MD5摘要。
    /// 
    /// </summary>
    public string ETag { get; internal set; }

    /// <summary>
    /// 获取一个值表示分块的标识
    /// 
    /// </summary>
    public int PartNumber { get; internal set; }

    /// <summary>
    /// 获取包含Part标识号码和ETag值的<see cref="P:Aliyun.OpenServices.OpenStorageService.UploadPartResult.PartETag"/>对象
    /// 
    /// </summary>
    public PartETag PartETag
    {
      get
      {
        return new PartETag(this.PartNumber, this.ETag);
      }
    }
  }
}
