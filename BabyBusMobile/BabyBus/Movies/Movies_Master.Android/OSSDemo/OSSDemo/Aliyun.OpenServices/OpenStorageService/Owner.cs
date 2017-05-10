// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Owner
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// 表示OSS实体的所有者。
  /// 
  /// </summary>
  [XmlRoot("Owner")]
  public class Owner : ICloneable
  {
    /// <summary>
    /// 获取或设置所有者的ID。
    /// 
    /// </summary>
    [XmlElement("ID")]
    public string Id { get; set; }

    /// <summary>
    /// 获取或设置所有者的显示名称。
    /// 
    /// </summary>
    [XmlElement("DisplayName")]
    public string DisplayName { get; set; }

    /// <summary>
    /// 构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.Owner"/>实例。
    /// 
    /// </summary>
    public Owner()
    {
    }

    /// <summary>
    /// 使用给定的所有者ID和显示名称构造一个新的<see cref="T:Aliyun.OpenServices.OpenStorageService.Owner"/>实例。
    /// 
    /// </summary>
    /// <param name="id">所有者的ID。</param><param name="displayName">所有者的显示名称。</param>
    public Owner(string id, string displayName)
    {
      this.Id = id;
      this.DisplayName = displayName;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[Owner Id={0}, DisplayName={1}]", new object[2]
      {
        (object) (this.Id ?? string.Empty),
        (object) (this.DisplayName ?? string.Empty)
      });
    }

    public object Clone()
    {
      return (object) new Owner(this.Id, this.DisplayName);
    }
  }
}
