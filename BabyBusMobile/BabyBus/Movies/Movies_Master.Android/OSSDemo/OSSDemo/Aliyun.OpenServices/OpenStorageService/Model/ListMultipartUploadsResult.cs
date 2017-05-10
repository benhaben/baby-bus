// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Model.ListMultipartUploadsResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
  /// <summary>
  /// Description of ListMultipartUploadsResult.
  /// 
  /// </summary>
  [XmlRoot("ListMultipartUploadsResult")]
  public class ListMultipartUploadsResult
  {
    [XmlElement("Bucket")]
    public string Bucket { get; set; }

    [XmlElement("KeyMarker")]
    public string KeyMarker { get; set; }

    [XmlElement("UploadIdMarker")]
    public string UploadIdMarker { get; set; }

    [XmlElement("NextKeyMarker")]
    public string NextKeyMarker { get; set; }

    [XmlElement("NextUploadIdMarker")]
    public string NextUploadIdMarker { get; set; }

    [XmlElement("Delimiter")]
    public string Delimiter { get; set; }

    [XmlElement("Prefix")]
    public string Prefix { get; set; }

    [XmlElement("MaxUploads")]
    public int MaxUploads { get; set; }

    [XmlElement("IsTruncated")]
    public bool IsTruncated { get; set; }

    [XmlElement("Upload")]
    public ListMultipartUploadsResult.Upload[] Uploads { get; set; }

    [XmlElement("CommonPrefixes")]
    public ListMultipartUploadsResult.CommonPrefixs CommonPrefix { get; set; }

    [XmlRoot("Upload")]
    public class Upload
    {
      [XmlElement("Key")]
      public string Key { get; set; }

      [XmlElement("UploadId")]
      public string UploadId { get; set; }

      [XmlElement("StorageClass")]
      public string StorageClass { get; set; }

      [XmlElement("Initiated")]
      public DateTime Initiated { get; set; }
    }

    [XmlRoot("CommonPrefixes")]
    public class CommonPrefixs
    {
      [XmlElement("Prefix")]
      public string[] Prefixs { get; set; }
    }
  }
}
