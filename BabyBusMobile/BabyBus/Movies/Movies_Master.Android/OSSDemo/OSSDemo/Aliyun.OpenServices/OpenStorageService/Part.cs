// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Part
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 获取Multipart Upload事件中某块数据的信息.
  /// 
  /// </summary>
  public class Part
  {
    /// <summary>
    /// 获取分块的编号
    /// 
    /// </summary>
    public int PartNumber { get; internal set; }

    /// <summary>
    /// 获取分块上传的时间
    /// 
    /// </summary>
    public DateTime LastModified { get; internal set; }

    /// <summary>
    /// 获取分块内容的ETag
    /// 
    /// </summary>
    public string ETag { get; internal set; }

    /// <summary>
    /// 获取分块的大小，单位字节
    /// 
    /// </summary>
    public long Size { get; internal set; }

    /// <summary>
    /// 获取包含Part标识号码和ETag值的<see cref="P:Aliyun.OpenServices.OpenStorageService.Part.PartETag"/>对象
    /// 
    /// </summary>
    public PartETag PartETag
    {
      get
      {
        return new PartETag(this.PartNumber, this.ETag);
      }
    }

    /// <summary>
    /// 获取该实例的字符串表示。
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[Part PartNumber={0}, ETag={1}, LastModified={2}, Size={3}]", (object) this.PartNumber, (object) this.ETag, (object) this.LastModified, (object) this.Size);
    }
  }
}
