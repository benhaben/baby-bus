// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Model.CompleteMultipartUploadRequestModel
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
  /// <summary>
  /// Description of CompleteUploadRequest.
  /// 
  /// </summary>
  [XmlRoot("CompleteMultipartUpload")]
  public class CompleteMultipartUploadRequestModel
  {
    [XmlElement("Part")]
    public CompleteMultipartUploadRequestModel.CompletePart[] Parts { get; set; }

    [XmlRoot("Part")]
    public class CompletePart
    {
      [XmlElement("PartNumber")]
      public int PartNumber { get; set; }

      [XmlElement("ETag")]
      public string ETag { get; set; }
    }
  }
}
