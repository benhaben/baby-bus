// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Utilities.HttpUtils
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Aliyun.OpenServices.Common.Utilities
{
  /// <summary>
  /// Description of HttpUtils.
  /// 
  /// </summary>
  internal static class HttpUtils
  {
    public const string Charset = "utf-8";
    public const string Iso88591Charset = "iso-8859-1";

    /// <summary>
    /// Builds the URI parameter string from the request parameters.
    /// 
    /// </summary>
    /// <param name="parameters"/>
    /// <returns/>
    public static string GetRequestParameterString(IEnumerable<KeyValuePair<string, string>> parameters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (KeyValuePair<string, string> keyValuePair in parameters)
      {
        Debug.Assert(!string.IsNullOrEmpty(keyValuePair.Key), "Null Or empty key is not allowed.");
        if (!flag)
          stringBuilder.Append("&");
        flag = false;
        stringBuilder.Append(keyValuePair.Key);
        if (keyValuePair.Value != null)
          stringBuilder.Append("=").Append(HttpUtils.UrlEncode(keyValuePair.Value, "utf-8"));
      }
      return ((object) stringBuilder).ToString();
    }

    /// <summary>
    /// Encodes the URL.
    /// 
    /// </summary>
    /// <param name="data"/>
    /// <returns/>
    public static string UrlEncode(string data, string charset)
    {
      Debug.Assert(data != null && !string.IsNullOrEmpty(charset));
      StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
      string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
      foreach (char ch in Encoding.GetEncoding(charset).GetBytes(data))
      {
        if (str.IndexOf(ch) != -1)
          stringBuilder.Append(ch);
        else
          stringBuilder.Append("%").Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:X2}", new object[1]
          {
            (object) (int) ch
          }));
      }
      return ((object) stringBuilder).ToString();
    }

    public static string ReEncode(string text, string fromCharset, string toCharset)
    {
      Debug.Assert(text != null);
      byte[] bytes = Encoding.GetEncoding(fromCharset).GetBytes(text);
      return Encoding.GetEncoding(toCharset).GetString(bytes);
    }
  }
}
