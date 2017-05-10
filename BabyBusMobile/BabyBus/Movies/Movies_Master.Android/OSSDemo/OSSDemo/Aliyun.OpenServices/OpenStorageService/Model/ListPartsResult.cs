// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Model.ListPartsResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
  /// <summary>
  /// Description of ListPartsResultModel.
  /// 
  /// </summary>
  [XmlRoot("ListPartsResult")]
  public class ListPartsResult
  {
    [XmlElement("Bucket")]
    public string Bucket { get; set; }

    [XmlElement("Key")]
    public string Key { get; set; }

    [XmlElement("UploadId")]
    public string UploadId { get; set; }

    [XmlElement("PartNumberMarker")]
    public int PartNumberMarker { get; set; }

    [XmlElement("NextPartNumberMarker")]
    public int NextPartNumberMarker { get; set; }

    [XmlElement("MaxParts")]
    public int MaxParts { get; set; }

    [XmlElement("IsTruncated")]
    public bool IsTruncated { get; set; }

    [XmlElement("Part")]
    public ListPartsResult.PartResult[] PartResults { get; set; }

    [XmlRoot("Part")]
    public class PartResult
    {
      [XmlElement("PartNumber")]
      public int PartNumber { get; set; }

      [XmlElement("LastModified")]
      public DateTime LastModified { get; set; }

      [XmlElement("ETag")]
      public string ETag { get; set; }

      [XmlElement("Size")]
      public long Size { get; set; }
    }
  }
}
