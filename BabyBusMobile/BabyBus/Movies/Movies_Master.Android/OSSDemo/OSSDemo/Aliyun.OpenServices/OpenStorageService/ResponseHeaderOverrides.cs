// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.ResponseHeaderOverrides
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 包含了在发送OSS GET请求时可以重载的返回请求头。
  /// 
  /// </summary>
  public class ResponseHeaderOverrides
  {
    internal const string ResponseHeaderContentType = "response-content-type";
    internal const string ResponseHeaderContentLanguage = "response-content-language";
    internal const string ResponseHeaderExpires = "response-expires";
    internal const string ResponseCacheControl = "response-cache-control";
    internal const string ResponseContentDisposition = "response-content-disposition";
    internal const string ResponseContentEncoding = "response-content-encoding";

    /// <summary>
    /// 获取或设置重载的Content-Type返回请求头。如果未指定，则返回null。
    /// 
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 获取或设置返回重载的Content-Language返回请求头。如果未指定，则返回null。
    /// 
    /// </summary>
    public string ContentLanguage { get; set; }

    /// <summary>
    /// 获取或设置返回重载的Expires返回请求头。如果未指定，则返回null。
    /// 
    /// </summary>
    public string Expires { get; set; }

    /// <summary>
    /// 获取或设置返回重载的Cache-Control返回请求头。如果未指定，则返回null。
    /// 
    /// </summary>
    public string CacheControl { get; set; }

    /// <summary>
    /// 获取或设置返回重载的Content-Disposition返回请求头。如果未指定，则返回null。
    /// 
    /// </summary>
    public string ContentDisposition { get; set; }

    /// <summary>
    /// 获取或设置返回重载的Content-Encoding返回请求头。如果未指定，则返回null。
    /// 
    /// </summary>
    public string ContentEncoding { get; set; }

    internal void Populate(IDictionary<string, string> parameters)
    {
      Debug.Assert(parameters != null);
      if (this.CacheControl != null)
        parameters.Add("response-cache-control", this.CacheControl);
      if (this.ContentDisposition != null)
        parameters.Add("response-content-disposition", this.ContentDisposition);
      if (this.ContentEncoding != null)
        parameters.Add("response-content-encoding", this.ContentEncoding);
      if (this.ContentLanguage != null)
        parameters.Add("response-content-language", this.ContentLanguage);
      if (this.ContentType != null)
        parameters.Add("response-content-type", this.ContentType);
      if (this.Expires == null)
        return;
      parameters.Add("response-expires", this.Expires);
    }
  }
}
