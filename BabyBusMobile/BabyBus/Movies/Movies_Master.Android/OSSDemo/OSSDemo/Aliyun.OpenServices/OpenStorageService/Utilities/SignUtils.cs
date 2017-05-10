// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Utilities.SignUtils
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
  /// <summary>
  /// Description of SignUtils.
  /// 
  /// </summary>
  internal class SignUtils
  {
    private static IList<string> SIGNED_PARAMTERS = (IList<string>) new List<string>()
    {
      "acl",
      "uploadId",
      "partNumber",
      "uploads",
      "cors",
      "logging",
      "website",
      "response-cache-control",
      "response-content-disposition",
      "response-content-encoding",
      "response-content-language",
      "response-content-type",
      "response-expires"
    };
    private const string _newLineMarker = "\n";

    public static string BuildCanonicalString(string method, string resourcePath, ServiceRequest request)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(method).Append("\n");
      IDictionary<string, string> headers = request.Headers;
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      if (headers != null)
      {
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) headers)
        {
          string key = keyValuePair.Key.ToLowerInvariant();
          if (key == "Content-Type".ToLowerInvariant() || key == "Content-MD5".ToLowerInvariant() || key == "Date".ToLowerInvariant() || key.StartsWith("x-oss-"))
            dictionary.Add(key, keyValuePair.Value);
        }
      }
      if (!dictionary.ContainsKey("Content-Type".ToLowerInvariant()))
        dictionary.Add("Content-Type".ToLowerInvariant(), "");
      if (!dictionary.ContainsKey("Content-MD5".ToLowerInvariant()))
        dictionary.Add("Content-MD5".ToLowerInvariant(), "");
      if (request.Parameters != null)
      {
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) request.Parameters)
        {
          if (keyValuePair.Key.StartsWith("x-oss-"))
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) Enumerable.OrderBy<KeyValuePair<string, string>, string>((IEnumerable<KeyValuePair<string, string>>) dictionary, (Func<KeyValuePair<string, string>, string>) (e => e.Key)))
      {
        string key = keyValuePair.Key;
        object obj = (object) keyValuePair.Value;
        if (key.StartsWith("x-oss-"))
          stringBuilder.Append(key).Append(':').Append(obj);
        else
          stringBuilder.Append(obj);
        stringBuilder.Append("\n");
      }
      stringBuilder.Append(SignUtils.BuildCanonicalizedResource(resourcePath, request.Parameters));
      return ((object) stringBuilder).ToString();
    }

    private static string BuildCanonicalizedResource(string resourcePath, IDictionary<string, string> parameters)
    {
      Debug.Assert(resourcePath.StartsWith("/"));
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(resourcePath);
      if (parameters != null)
      {
        IOrderedEnumerable<string> orderedEnumerable = Enumerable.OrderBy<string, string>((IEnumerable<string>) parameters.Keys, (Func<string, string>) (e => e));
        char ch = '?';
        foreach (string index in (IEnumerable<string>) orderedEnumerable)
        {
          if (SignUtils.SIGNED_PARAMTERS.Contains(index))
          {
            stringBuilder.Append(ch);
            stringBuilder.Append(index);
            string str = parameters[index];
            if (str != null)
              stringBuilder.Append("=").Append(str);
            ch = '&';
          }
        }
      }
      return ((object) stringBuilder).ToString();
    }
  }
}
