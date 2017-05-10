// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.ObjectMetadata
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// OSS中Object的元数据。
  /// 
  /// <para>
  /// 包含了用户自定义的元数据，也包含了OSS发送的标准HTTP头(如Content-Length, ETag等）。
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ObjectMetadata
  {
    private IDictionary<string, string> _userMetadata = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private IDictionary<string, object> _metadata = (IDictionary<string, object>) new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private const string DefaultObjectContentType = "application/octet-stream";

    /// <summary>
    /// 获取用户自定义的元数据。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// OSS内部保存用户自定义的元数据时，会以x-oss-meta-为请求头的前缀。
    ///             但用户通过该接口处理用户自定义元数据里，不需要加上前缀“x-oss-meta-”。
    ///             同时，元数据字典的键名是不区分大小写的，并且在从服务器端返回时会全部以小写形式返回，
    ///             即使在设置时给定了大写字母。比如键名为：MyUserMeta，通过GetObjectMetadata接口
    ///             返回时键名会变为：myusermeta。
    /// 
    /// </remarks>
    public IDictionary<string, string> UserMetadata
    {
      get
      {
        return this._userMetadata;
      }
    }

    /// <summary>
    /// 获取Last-Modified请求头的值，表示Object最后一次修改的时间。
    /// 
    /// </summary>
    public DateTime LastModified
    {
      get
      {
        return this._metadata.ContainsKey("Last-Modified") ? (DateTime) this._metadata["Last-Modified"] : DateTime.MinValue;
      }
      internal set
      {
        this._metadata["Last-Modified"] = (object) value;
      }
    }

    /// <summary>
    /// 获取Expires请求头，表示Object的过期时间。
    ///             如果Object没有定义过期时间，则返回null。
    /// 
    /// </summary>
    public DateTime ExpirationTime
    {
      get
      {
        return this._metadata.ContainsKey("Expires") ? (DateTime) this._metadata["Expires"] : DateTime.MinValue;
      }
      internal set
      {
        this._metadata["Expires"] = (object) value;
      }
    }

    /// <summary>
    /// 获取Content-Length请求头，表示Object内容的大小。
    /// 
    /// </summary>
    public long ContentLength
    {
      get
      {
        return this._metadata.ContainsKey("Content-Length") ? (long) this._metadata["Content-Length"] : 0L;
      }
      set
      {
        this._metadata["Content-Length"] = (object) value;
      }
    }

    /// <summary>
    /// 获取或设置Content-Type请求头，表示Object内容的类型，为标准的MIME类型。
    /// 
    /// </summary>
    public string ContentType
    {
      get
      {
        return this._metadata.ContainsKey("Content-Type") ? this._metadata["Content-Type"] as string : (string) null;
      }
      set
      {
        this._metadata["Content-Type"] = (object) value;
      }
    }

    /// <summary>
    /// 获取或设置Content-Encoding请求头，表示Object内容的编码方式。
    /// 
    /// </summary>
    public string ContentEncoding
    {
      get
      {
        return this._metadata.ContainsKey("Content-Encoding") ? this._metadata["Content-Encoding"] as string : (string) null;
      }
      set
      {
        this._metadata["Content-Encoding"] = (object) value;
      }
    }

    /// <summary>
    /// 获取或设置Cache-Control请求头，表示用户指定的HTTP请求/回复链的缓存行为。
    /// 
    /// </summary>
    public string CacheControl
    {
      get
      {
        return this._metadata.ContainsKey("Cache-Control") ? this._metadata["Cache-Control"] as string : (string) null;
      }
      set
      {
        this._metadata["Cache-Control"] = (object) value;
      }
    }

    /// <summary>
    /// 获取Content-Disposition请求头，表示MIME用户代理如何显示附加的文件。
    /// 
    /// </summary>
    public string ContentDisposition
    {
      get
      {
        return this._metadata.ContainsKey("Content-Disposition") ? this._metadata["Content-Disposition"] as string : (string) null;
      }
      set
      {
        this._metadata["Content-Disposition"] = (object) value;
      }
    }

    /// <summary>
    /// 获取或设置一个值表示与Object相关的hex编码的128位MD5摘要。
    /// 
    /// </summary>
    public string ETag
    {
      get
      {
        return this._metadata.ContainsKey("ETag") ? this._metadata["ETag"] as string : (string) null;
      }
      set
      {
        this._metadata["ETag"] = (object) value;
      }
    }

    /// <summary>
    /// 初始化一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.ObjectMetadata"/>实例。
    /// 
    /// </summary>
    public ObjectMetadata()
    {
      this.ContentLength = -1L;
    }

    internal void AddHeader(string key, object value)
    {
      Debug.Assert(!string.IsNullOrEmpty(key));
      this._metadata.Add(key, value);
    }

    /// <summary>
    /// Populates the request header dictionary with the metdata and user metadata.
    /// 
    /// </summary>
    /// <param name="requestHeaders"/>
    internal void Populate(IDictionary<string, string> requestHeaders)
    {
      Debug.Assert(requestHeaders != null);
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) this._metadata)
        requestHeaders.Add(keyValuePair.Key, keyValuePair.Value.ToString());
      if (!requestHeaders.ContainsKey("Content-Type"))
        requestHeaders.Add("Content-Type", "application/octet-stream");
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this._userMetadata)
        requestHeaders.Add("x-oss-meta-" + keyValuePair.Key, keyValuePair.Value);
    }
  }
}
