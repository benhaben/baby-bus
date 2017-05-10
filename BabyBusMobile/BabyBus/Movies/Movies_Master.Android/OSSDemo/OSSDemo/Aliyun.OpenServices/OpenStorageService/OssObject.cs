// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.OssObject
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Properties;
using System;
using System.Globalization;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 表示OSS中的Object。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// 在 OSS 中，用户的每个文件都是一个 Object，每个文件需小于 5G。
  ///             Object包含key、data和user meta。其中，key是Object 的名字；
  ///             data是Object 的数据；user meta是用户对该object的描述。
  /// 
  /// </para>
  /// 
  /// </remarks>
  public class OssObject : IDisposable
  {
    private ObjectMetadata _metadata = new ObjectMetadata();
    private bool _disposed;
    private string _key;

    /// <summary>
    /// 获取或设置Object的Key。
    /// 
    /// </summary>
    public string Key
    {
      get
      {
        return this._key;
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "value");
        this._key = value;
      }
    }

    /// <summary>
    /// 获取或设置Object所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 获取Object的元数据。
    /// 
    /// </summary>
    public ObjectMetadata Metadata
    {
      get
      {
        return this._metadata;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");
        this._metadata = value;
      }
    }

    /// <summary>
    /// 获取或设置Object内容的数据流。
    /// 
    /// </summary>
    public Stream Content { get; set; }

    /// <summary>
    /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>实例。
    /// 
    /// </summary>
    public OssObject(string key)
    {
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      this._key = key;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[OSSObject Key={0}, BucketName={1}]", new object[2]
      {
        (object) this._key,
        (object) (this.BucketName ?? string.Empty)
      });
    }

    /// <summary>
    /// 释放<see cref="P:Aliyun.OpenServices.OpenStorageService.OssObject.Content"/>。
    /// 
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (this.Content != null)
        this.Content.Dispose();
      this._disposed = true;
    }
  }
}
