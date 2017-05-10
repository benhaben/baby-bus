// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Model.InternalTableMeta
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
  [XmlRoot(ElementName = "TableMeta")]
  public class InternalTableMeta
  {
    public string TableName { get; set; }

    public int PagingKeyLen { get; set; }

    public string TableGroupName { get; set; }

    [XmlElement(ElementName = "View")]
    public List<InternalViewMeta> Views { get; set; }

    [XmlElement(ElementName = "PrimaryKey")]
    public List<PrimaryKey> PrimaryKeys { get; set; }

    public InternalTableMeta()
    {
      this.PrimaryKeys = new List<PrimaryKey>();
      this.Views = new List<InternalViewMeta>();
    }

    internal TableMeta ToTableMeta()
    {
      TableMeta tableMeta = new TableMeta(this.TableName);
      tableMeta.PagingKeyLength = this.PagingKeyLen;
      tableMeta.TableGroupName = this.TableGroupName;
      foreach (PrimaryKey primaryKey in this.PrimaryKeys)
        tableMeta.PrimaryKeys.Add(primaryKey.Name, PrimaryKeyTypeHelper.Parse(primaryKey.PrimaryKeyType));
      foreach (InternalViewMeta internalViewMeta in this.Views)
        tableMeta.Views.Add(internalViewMeta.ToOpenTableViewMeta());
      return tableMeta;
    }
  }
}
