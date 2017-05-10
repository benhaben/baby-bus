// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Utilities.OssHeaders
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
  /// <summary>
  /// Description of OSSHeaders.
  /// 
  /// </summary>
  internal class OssHeaders
  {
    public const string OssPrefix = "x-oss-";
    public const string OssUserMetaPrefix = "x-oss-meta-";
    public const string OssCannedAcl = "x-oss-acl";
    public const string OssStorageClass = "x-oss-storage-class";
    public const string OssVersionId = "x-oss-version-id";
    public const string GetObjectIfModifiedSince = "If-Modified-Since";
    public const string GetObjectIfUnmodifiedSince = "If-Unmodified-Since";
    public const string GetObjectIfMatch = "If-Match";
    public const string GetObjectIfNoneMatch = "If-None-Match";
    public const string CopyObjectSource = "x-oss-copy-source";
    public const string CopyObjectSourceIfMatch = "x-oss-copy-source-if-match";
    public const string CopyObjectSourceIfNoneMatch = "x-oss-copy-source-if-none-match";
    public const string CopyObjectSourceIfUnmodifiedSince = "x-oss-copy-source-if-unmodified-since";
    public const string CopyObjectSourceIfModifedSince = "x-oss-copy-source-if-modified-since";
    public const string CopyObjectMetaDataDirective = "x-oss-metadata-directive";
    public const string OptionsOrigin = "Origin";
    public const string AccessControlRequestMethod = "Access-Control-Request-Method";
    public const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
  }
}
