// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.ServiceRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aliyun.OpenServices.Common.Communication
{
  /// <summary>
  /// Represents the information for sending requests.
  /// 
  /// </summary>
  internal class ServiceRequest : ServiceMessage, IDisposable
  {
    private IDictionary<string, string> parameters = (IDictionary<string, string>) new Dictionary<string, string>();
    private bool _disposed;

    /// <summary>
    /// Gets or sets the endpoint.
    /// 
    /// </summary>
    public Uri Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the resource path of the request URI.
    /// 
    /// </summary>
    public string ResourcePath { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method.
    /// 
    /// </summary>
    public HttpMethod Method { get; set; }

    /// <summary>
    /// Gets the dictionary of the request parameters.
    /// 
    /// </summary>
    public IDictionary<string, string> Parameters
    {
      get
      {
        return this.parameters;
      }
    }

    /// <summary>
    /// Gets whether the request can be repeated.
    /// 
    /// </summary>
    public bool IsRepeatable
    {
      get
      {
        return this.Content == null || this.Content.CanSeek;
      }
    }

    /// <summary>
    /// Build the request URI from the request message.
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public string BuildRequestUri()
    {
      string str = this.Endpoint.ToString();
      if (!str.EndsWith("/") && (this.ResourcePath == null || !this.ResourcePath.StartsWith("/")))
        str = str + "/";
      if (this.ResourcePath != null)
        str = str + this.ResourcePath;
      if (this.IsParameterInUri())
      {
        string requestParameterString = HttpUtils.GetRequestParameterString((IEnumerable<KeyValuePair<string, string>>) this.parameters);
        if (!string.IsNullOrEmpty(requestParameterString))
          str = str + "?" + requestParameterString;
      }
      Console.WriteLine(str);
      return str;
    }

    public Stream BuildRequestContent()
    {
      if (!this.IsParameterInUri())
      {
        string requestParameterString = HttpUtils.GetRequestParameterString((IEnumerable<KeyValuePair<string, string>>) this.parameters);
        if (!string.IsNullOrEmpty(requestParameterString))
        {
          byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(requestParameterString);
          Stream stream = (Stream) new MemoryStream();
          stream.Write(bytes, 0, bytes.Length);
          stream.Flush();
          stream.Seek(0L, SeekOrigin.Begin);
          return stream;
        }
      }
      return this.Content;
    }

    private bool IsParameterInUri()
    {
      return this.Method != HttpMethod.Post || this.Content != null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed || !disposing)
        return;
      if (this.Content != null)
      {
        this.Content.Close();
        this.Content = (Stream) null;
      }
      this._disposed = true;
    }
  }
}
