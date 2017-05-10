// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.HttpFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Aliyun.OpenServices.Common.Communication
{
  internal static class HttpFactory
  {
    internal static HttpWebRequest CreateWebRequest(ServiceRequest serviceRequest, ClientConfiguration configuration)
    {
      Debug.Assert(serviceRequest != null && configuration != null);
      HttpWebRequest webRequest = WebRequest.Create(serviceRequest.BuildRequestUri()) as HttpWebRequest;
      HttpFactory.SetRequestHeaders(webRequest, serviceRequest, configuration);
      HttpFactory.SetRequestProxy(webRequest, configuration);
      return webRequest;
    }

    private static void SetRequestHeaders(HttpWebRequest webRequest, ServiceRequest serviceRequest, ClientConfiguration configuration)
    {
      webRequest.Timeout = configuration.ConnectionTimeout;
      webRequest.Method = ((object) serviceRequest.Method).ToString().ToUpperInvariant();
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) serviceRequest.Headers)
        HttpExtensions.AddInternal(webRequest.Headers, keyValuePair.Key, keyValuePair.Value);
      if (string.IsNullOrEmpty(configuration.UserAgent))
        return;
      webRequest.UserAgent = configuration.UserAgent;
    }

    private static void SetRequestProxy(HttpWebRequest webRequest, ClientConfiguration configuration)
    {
      webRequest.Proxy = (IWebProxy) null;
      if (string.IsNullOrEmpty(configuration.ProxyHost))
        return;
      if (configuration.ProxyPort < 0)
        webRequest.Proxy = (IWebProxy) new WebProxy(configuration.ProxyHost);
      else
        webRequest.Proxy = (IWebProxy) new WebProxy(configuration.ProxyHost, configuration.ProxyPort);
      if (!string.IsNullOrEmpty(configuration.ProxyUserName))
        webRequest.Proxy.Credentials = string.IsNullOrEmpty(configuration.ProxyDomain) ? (ICredentials) new NetworkCredential(configuration.ProxyUserName, configuration.ProxyPassword ?? string.Empty) : (ICredentials) new NetworkCredential(configuration.ProxyUserName, configuration.ProxyPassword ?? string.Empty, configuration.ProxyDomain);
    }
  }
}
