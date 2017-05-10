// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Model.SetBucketWebsiteRequestModel
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService.Model
{
  [XmlRoot("WebsiteConfiguration")]
  public class SetBucketWebsiteRequestModel
  {
    [XmlElement("IndexDocument")]
    public SetBucketWebsiteRequestModel.IndexDocumentModel IndexDocument { get; set; }

    [XmlElement("ErrorDocument")]
    public SetBucketWebsiteRequestModel.ErrorDocumentModel ErrorDocument { get; set; }

    [XmlRoot("IndexDocument")]
    public class IndexDocumentModel
    {
      [XmlElement("Suffix")]
      public string Suffix { get; set; }
    }

    [XmlRoot("ErrorDocument")]
    public class ErrorDocumentModel
    {
      [XmlElement("Key")]
      public string Key { get; set; }
    }
  }
}
