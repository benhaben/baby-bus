// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.ClientConfiguration
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;

namespace Aliyun.OpenServices
{
  /// <summary>
  /// 表示访问阿里云服务的配置信息。
  /// 
  /// </summary>
  public class ClientConfiguration : ICloneable
  {
    private static string _defaultUserAgent = "aliyun-sdk-dotnet/" + ((object) typeof (ClientConfiguration).Assembly.GetName().Version).ToString();
    private string _userAgent = ClientConfiguration._defaultUserAgent;
    private int _proxyPort = -1;
    private int _connectionTimeout = 60000;
    private int _maxErrorRetry = 3;

    /// <summary>
    /// 获取设置访问请求的User-Agent。
    /// 
    /// </summary>
    public string UserAgent
    {
      get
      {
        return this._userAgent;
      }
      set
      {
        this._userAgent = value;
      }
    }

    /// <summary>
    /// 获取或设置代理服务器的地址。
    /// 
    /// </summary>
    public string ProxyHost { get; set; }

    /// <summary>
    /// 获取或设置代理服务器的端口。
    /// 
    /// </summary>
    public int ProxyPort
    {
      get
      {
        return this._proxyPort;
      }
      set
      {
        this._proxyPort = value;
      }
    }

    /// <summary>
    /// 获取或设置用户名。
    /// 
    /// </summary>
    public string ProxyUserName { get; set; }

    /// <summary>
    /// 获取或设置密码。
    /// 
    /// </summary>
    public string ProxyPassword { get; set; }

    /// <summary>
    /// 获取或设置代理服务器授权用户所在的域。
    /// 
    /// </summary>
    public string ProxyDomain { get; set; }

    /// <summary>
    /// 获取或设置连接的超时时间，单位为毫秒。
    /// 
    /// </summary>
    public int ConnectionTimeout
    {
      get
      {
        return this._connectionTimeout;
      }
      set
      {
        this._connectionTimeout = value;
      }
    }

    /// <summary>
    /// 获取或设置请求发生错误时最大的重试次数。
    /// 
    /// </summary>
    public int MaxErrorRetry
    {
      get
      {
        return this._maxErrorRetry;
      }
      set
      {
        this._maxErrorRetry = value;
      }
    }

    /// <summary>
    /// 获取该实例的拷贝。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 该实例的拷贝。
    /// </returns>
    public object Clone()
    {
      return (object) new ClientConfiguration()
      {
        ConnectionTimeout = this.ConnectionTimeout,
        MaxErrorRetry = this.MaxErrorRetry,
        ProxyDomain = this.ProxyDomain,
        ProxyHost = this.ProxyHost,
        ProxyPassword = this.ProxyPassword,
        ProxyPort = this.ProxyPort,
        ProxyUserName = this.ProxyUserName,
        UserAgent = this.UserAgent
      };
    }
  }
}
