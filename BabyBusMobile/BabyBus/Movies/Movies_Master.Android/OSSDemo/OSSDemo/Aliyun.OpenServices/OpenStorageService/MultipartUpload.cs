// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.MultipartUpload
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 获取MultipartUpload事件的信息.
  /// 
  /// </summary>
  public class MultipartUpload
  {
    /// <summary>
    /// 获取Object的key
    /// 
    /// </summary>
    public string Key { get; internal set; }

    /// <summary>
    /// 获取上传Id
    /// 
    /// </summary>
    public string UploadId { get; internal set; }

    /// <summary>
    /// 获取Object的存储类别。
    /// 
    /// </summary>
    public string StorageClass { get; internal set; }

    /// <summary>
    /// Multipart Upload事件初始化的时间。
    /// 
    /// </summary>
    public DateTime Initiated { get; internal set; }

    internal MultipartUpload()
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
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[MultipartUpload Key={0}, UploadId={1}]", new object[2]
      {
        (object) this.Key,
        (object) this.UploadId
      });
    }
  }
}
