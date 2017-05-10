// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.ObjectListing
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 包含获取OSS的<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>中<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>列表的信息。
  /// 
  /// </summary>
  public class ObjectListing
  {
    private IList<OssObjectSummary> _objectSummaries = (IList<OssObjectSummary>) new List<OssObjectSummary>();
    private IList<string> _commonPrefixes = (IList<string>) new List<string>();

    /// <summary>
    /// 获取<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在<see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。
    /// 
    /// </summary>
    public string BucketName { get; private set; }

    /// <summary>
    /// 获取一个值表示用于下一个<see cref="P:ListObjectRequest.Marker"/>以读取
    ///             结果列表的下一页。
    ///             如果结果列表没有被截取掉，则该属性返回null。
    /// 
    /// </summary>
    public string NextMarker { get; internal set; }

    /// <summary>
    /// 获取一个值表示结果列表有没有被截取掉。
    /// 
    /// </summary>
    public bool IsTrunked { get; internal set; }

    /// <summary>
    /// 获取请求参数<see cref="P:ListObjectRequest.Marker"/>的值。
    /// 
    /// </summary>
    public string Marker { get; internal set; }

    /// <summary>
    /// 获取请求参数<see cref="P:ListObjectRequest.MaxKeys"/>的值。
    /// 
    /// </summary>
    public int MaxKeys { get; internal set; }

    /// <summary>
    /// 获取请求参数<see cref="P:ListObjectRequest.Prefix"/>的值。
    /// 
    /// </summary>
    public string Prefix { get; internal set; }

    /// <summary>
    /// 获取请求参数<see cref="P:ListObjectRequest.Delimiter"/>的值。
    /// 
    /// </summary>
    public string Delimiter { get; internal set; }

    /// <summary>
    /// 枚举满足查询条件的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObjectSummary"/>。
    /// 
    /// </summary>
    public IEnumerable<OssObjectSummary> ObjectSummaries
    {
      get
      {
        return (IEnumerable<OssObjectSummary>) this._objectSummaries;
      }
    }

    /// <summary>
    /// 获取返回结果中的CommonPrefixes部分。
    /// 
    /// </summary>
    public IEnumerable<string> CommonPrefixes
    {
      get
      {
        return (IEnumerable<string>) this._commonPrefixes;
      }
    }

    /// <summary>
    /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.ObjectListing"/>实例。
    /// 
    /// </summary>
    /// <param name="bucketName"><see cref="T:Aliyun.OpenServices.OpenStorageService.Bucket"/>的名称。</param>
    internal ObjectListing(string bucketName)
    {
      if (string.IsNullOrEmpty(bucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
      if (!OssUtils.IsBucketNameValid(bucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");
      this.BucketName = bucketName;
    }

    internal void AddObjectSummary(OssObjectSummary summary)
    {
      this._objectSummaries.Add(summary);
    }

    internal void AddCommonPrefix(string prefix)
    {
      this._commonPrefixes.Add(prefix);
    }
  }
}
