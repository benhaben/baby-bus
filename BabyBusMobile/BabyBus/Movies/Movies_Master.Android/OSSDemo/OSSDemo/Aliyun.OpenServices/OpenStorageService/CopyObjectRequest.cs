// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.CopyObjectRequest
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 指定拷贝Object的请求参数
  /// 
  /// </summary>
  public class CopyObjectRequest
  {
    private IList<string> _matchingETagConstraints = (IList<string>) new List<string>();
    private IList<string> _nonmatchingETagConstraints = (IList<string>) new List<string>();

    /// <summary>
    /// 获取或者设置源Object所在的Bucket的名称。
    /// 
    /// </summary>
    public string SourceBucketName { get; set; }

    /// <summary>
    /// 获取或者设置源Object的Key。
    /// 
    /// </summary>
    public string SourceKey { get; set; }

    /// <summary>
    /// 获取或者设置目标Object所在的Bucket的名称。
    /// 
    /// </summary>
    public string DestinationBucketName { get; set; }

    /// <summary>
    /// 获取或者设置目标Object的Key。
    /// 
    /// </summary>
    public string DestinationKey { get; set; }

    /// <summary>
    /// 获取或者设置目标Object的Metadata信息。
    /// 
    /// </summary>
    public ObjectMetadata NewObjectMetaData { get; set; }

    /// <summary>
    /// 如果源Object的ETAG值和用户提供的ETAG相等，则执行拷贝操作；否则返回412 HTTP错误码（预处理失败）。
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
    /// 如果源Object的ETAG值和用户提供的ETAG不相等，则执行拷贝操作；否则返回412 HTTP错误码（预处理失败）。
    /// 
    /// </summary>
    public IList<string> NonmatchingETagConstraints
    {
      get
      {
        return this._nonmatchingETagConstraints;
      }
    }

    /// <summary>
    /// 如果传入参数中的时间等于或者晚于文件实际修改时间，则正常传输文件，并返回200 OK；
    ///             否则返回412 precondition failed错误
    /// 
    /// </summary>
    public DateTime? UnmodifiedSinceConstraint { get; set; }

    /// <summary>
    /// 如果源Object自从用户指定的时间以后被修改过，则执行拷贝操作；
    ///             否则返回412 HTTP错误码（预处理失败）。
    /// 
    /// </summary>
    public DateTime? ModifiedSinceConstraint { get; set; }

    /// <summary>
    /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.CopyObjectRequest"/> 实例
    /// 
    /// </summary>
    /// <param name="sourceBucketName">需要拷贝的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在的Bucket</param><param name="sourceKey">需要拷贝的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>名称</param><param name="destinationBucketName">要拷贝到的目的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>所在的Bucket</param><param name="destinationKey">要拷贝到的目的<see cref="T:Aliyun.OpenServices.OpenStorageService.OssObject"/>的名称</param>
    public CopyObjectRequest(string sourceBucketName, string sourceKey, string destinationBucketName, string destinationKey)
    {
      if (string.IsNullOrEmpty(sourceBucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceBucketName");
      if (string.IsNullOrEmpty(sourceKey))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "sourceKey");
      if (string.IsNullOrEmpty(destinationBucketName))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationBucketName");
      if (string.IsNullOrEmpty(destinationKey))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "destinationKey");
      if (!OssUtils.IsBucketNameValid(sourceBucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "sourceBucketName");
      if (!OssUtils.IsObjectKeyValid(sourceKey))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "sourceKey");
      if (!OssUtils.IsBucketNameValid(destinationBucketName))
        throw new ArgumentException(OssResources.BucketNameInvalid, "destinationBucketName");
      if (!OssUtils.IsObjectKeyValid(destinationKey))
        throw new ArgumentException(OssResources.ObjectKeyInvalid, "destinationKey");
      this.SourceBucketName = sourceBucketName;
      this.SourceKey = sourceKey;
      this.DestinationBucketName = destinationBucketName;
      this.DestinationKey = destinationKey;
    }

    internal void Populate(IDictionary<string, string> headers)
    {
      string str = "/" + this.SourceBucketName + "/" + this.SourceKey;
      headers.Add("x-oss-copy-source", str);
      if (this.ModifiedSinceConstraint.HasValue)
        headers.Add("x-oss-copy-source-if-modified-since", DateUtils.FormatRfc822Date(this.ModifiedSinceConstraint.Value));
      if (this.UnmodifiedSinceConstraint.HasValue)
        headers.Add("x-oss-copy-source-if-unmodified-since", DateUtils.FormatRfc822Date(this.UnmodifiedSinceConstraint.Value));
      if (this._matchingETagConstraints.Count > 0)
        headers.Add("x-oss-copy-source-if-match", CopyObjectRequest.JoinETag((IEnumerable<string>) this._matchingETagConstraints));
      if (this._nonmatchingETagConstraints.Count > 0)
        headers.Add("x-oss-copy-source-if-none-match", CopyObjectRequest.JoinETag((IEnumerable<string>) this._nonmatchingETagConstraints));
      if (this.NewObjectMetaData != null)
      {
        headers.Add("x-oss-metadata-directive", "REPLACE");
        this.NewObjectMetaData.Populate(headers);
      }
      headers.Remove("Content-Length");
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
