// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Utilities.OssUtils
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
  /// <summary>
  /// Description of OssUtils.
  /// 
  /// </summary>
  internal static class OssUtils
  {
    public static readonly Uri DefaultEndpoint = new Uri("http://oss.aliyuncs.com");
    public const string Charset = "utf-8";
    public const int BufferSize = 8192;
    public const long MaxFileSzie = 5368709120L;

    public static bool IsBucketNameValid(string bucketName)
    {
      Debug.Assert(!string.IsNullOrEmpty(bucketName));
      return new Regex("^[a-z0-9][a-z0-9_\\-]{2,254}$").Match(bucketName).Success;
    }

    public static bool IsObjectKeyValid(string key)
    {
      Debug.Assert(!string.IsNullOrEmpty(key));
      int byteCount = Encoding.GetEncoding("utf-8").GetByteCount(key);
      return byteCount > 0 && byteCount < 1024;
    }

    public static string MakeResourcePath(string key)
    {
      return key == null ? string.Empty : OssUtils.UrlEncodeKey(key);
    }

    public static string MakeResourcePath(string bucket, string key)
    {
      if (bucket != null)
        return bucket + (key != null ? "/" + OssUtils.UrlEncodeKey(key) : string.Empty);
      else
        return string.Empty;
    }

    public static Uri MakeBucketEndpoint(Uri endpoint, string bucket)
    {
      return new Uri(endpoint.Scheme + "://" + (bucket != null ? bucket + "." : "") + endpoint.Host + (endpoint.Port != 80 ? ":" + (object) endpoint.Port : ""));
    }

    private static string UrlEncodeKey(string key)
    {
      string[] strArray = key.Split('/');
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(HttpUtils.UrlEncode(strArray[0], "utf-8"));
      for (int index = 1; index < strArray.Length; ++index)
        stringBuilder.Append('/').Append(HttpUtils.UrlEncode(strArray[index], "utf-8"));
      if (key.EndsWith('/'.ToString()))
      {
        string str = key;
        for (int index = 0; index < str.Length && (int) str[index] == 47; ++index)
          stringBuilder.Append('/');
      }
      return ((object) stringBuilder).ToString();
    }

    public static string TrimETag(string eTag)
    {
      string str;
      if (eTag == null)
        str = (string) null;
      else
        str = eTag.Trim('"');
      return str;
    }
  }
}
