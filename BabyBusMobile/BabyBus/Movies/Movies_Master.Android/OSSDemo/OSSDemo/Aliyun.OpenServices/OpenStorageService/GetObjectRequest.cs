// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.GetObjectRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 指定从OSS下载Object的请求参数。
  /// 
  /// </summary>
  public class GetObjectRequest
  {
    private IList<string> _matchingETagConstraints = (IList<string>) new List<string>();
    private IList<string> _nonmatchingEtagConstraints = (IList<string>) new List<string>();
    private ResponseHeaderOverrides _responseHeaders = new ResponseHeaderOverrides();

    /// <summary>
    /// 获取或设置<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// 获取或设置要下载<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的Key。
    /// 
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// 获取表示请求应当返回<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>内容的字节范围。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 调用<see cref="M:Aliyun.OpenServices.OpenStorageService.GetObjectRequest.SetRange(System.Int64,System.Int64)"/>方法进行设置，如果没有设置，则返回null。
    /// 
    /// </remarks>
    public long[] Range { get; private set; }

    /// <summary>
    /// 获取或设置“If-Unmodified-Since”参数。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 该参数表示：如果传入参数中的时间等于或者晚于文件实际修改时间，则传送文件；
    ///             如果早于实际修改时间，则返回错误。
    /// 
    /// </remarks>
    public DateTime? UnmodifiedSinceConstraint { get; set; }

    /// <summary>
    /// 获取或设置“If-Modified-Since”参数。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 该参数表示：如果指定的时间早于实际修改时间，则正常传送文件，并返回 200 OK；
    ///             如果参数中的时间和实际修改时间一样或者更晚，会返回错误。
    /// 
    /// </remarks>
    public DateTime? ModifiedSinceConstraint { get; set; }

    /// <summary>
    /// 获取一个列表表示：如果传入期望的ETag和<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的ETag匹配，则正常的发送文件。
    ///             如果不符合，返回错误。
    ///             对应“If-Match”参数，
    /// 
    /// </summary>
    public IList<string> MatchingETagConstraints
    {
      get
      {
        return this._matchingETagConstraints;
      }
    }

    /// <summary>
    /// 获取一个列表表示：如果传入期望的ETag和<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的ETag不匹配，则正常的发送文件。
    ///             如果符合，返回错误。
    ///             对应“If-None-Match”参数，
    /// 
    /// </summary>
    public IList<string> NonmatchingETagConstraints
    {
      get
      {
        return this._nonmatchingEtagConstraints;
      }
    }

    /// <summary>
    /// 获取的返回请求头重载<see cref="T:Aliyun.OpenServices.OpenStorageService.ResponseHeaderOverrides"/>实例。
    /// 
    /// </summary>
    public ResponseHeaderOverrides ResponseHeaders
    {
      get
      {
        return this._responseHeaders;
      }
    }

    /// <summary>
    /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.GetObjectRequest"/>实例。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param><param name="key"><see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的<see cref="P:OssObject.Key"/>。</param>
    public GetObjectRequest(string bucketName, string key)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      if (!OssUtils.IsObjectKeyValid(key))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "key");
      this.BucketName = bucketName;
      this.Key = key;
    }

    /// <summary>
    /// 设置一个值表示请求应当返回Object内容的字节范围（可选）。
    /// 
    /// </summary>
    /// <param name="start">范围的起始值。
    /// 
    /// <para>
    /// 当值大于或等于0时，表示起始的字节位置。
    ///             当值为-1时，表示不设置起始的字节位置，此时end参数不能-1，
    ///             例如end为100，Range请求头的值为bytes=-100，表示获取最后100个字节。
    /// 
    /// </para>
    /// </param><param name="end">范围的结束值，应当小于内容的字节数。（最大为内容的字节数-1）
    /// 
    /// <para>
    /// 当值小于或等于0时，表示结束的字节位或最后的字节数。
    ///             当值为-1时，表示不设置结束的字节位置，此时start参数不能为-1，
    ///             例如start为99，Range请求头的值为bytes=99-，表示获取第100个字节及
    ///             以后的所有内容。
    /// 
    /// </para>
    /// </param>
    public void SetRange(long start, long end)
    {
      this.Range = new long[2]
      {
        start,
        end
      };
    }

    internal void Populate(IDictionary<string, string> headers)
    {
      Debug.Assert(headers != null);
      if (this.Range != null && (this.Range[0] >= 0L || this.Range[1] >= 0L))
      {
        StringBuilder stringBuilder = new StringBuilder().Append("bytes=");
        if (this.Range[0] >= 0L)
          stringBuilder.Append(this.Range[0].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        stringBuilder.Append("-");
        if (this.Range[1] >= 0L)
          stringBuilder.Append(this.Range[1].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        headers.Add("Range", ((object) stringBuilder).ToString());
      }
      DateTime? nullable;
      if (this.ModifiedSinceConstraint.HasValue)
      {
        IDictionary<string, string> dictionary = headers;
        string key = "If-Modified-Since";
        nullable = this.ModifiedSinceConstraint;
        string str = DateUtils.FormatRfc822Date(nullable.Value);
        dictionary.Add(key, str);
      }
      nullable = this.UnmodifiedSinceConstraint;
      if (nullable.HasValue)
      {
        IDictionary<string, string> dictionary = headers;
        string key = "If-Unmodified-Since";
        nullable = this.UnmodifiedSinceConstraint;
        string str = DateUtils.FormatRfc822Date(nullable.Value);
        dictionary.Add(key, str);
      }
      if (this._matchingETagConstraints.Count > 0)
        headers.Add("If-Match", GetObjectRequest.JoinETag((IEnumerable<string>) this._matchingETagConstraints));
      if (this._nonmatchingEtagConstraints.Count <= 0)
        return;
      headers.Add("If-None-Match", GetObjectRequest.JoinETag((IEnumerable<string>) this._nonmatchingEtagConstraints));
    }

    private static string JoinETag(IEnumerable<string> etags)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (string str in etags)
      {
        if (!flag)
          stringBuilder.Append(", ");
        stringBuilder.Append(str);
        flag = false;
      }
      return ((object) stringBuilder).ToString();
    }
  }
}
