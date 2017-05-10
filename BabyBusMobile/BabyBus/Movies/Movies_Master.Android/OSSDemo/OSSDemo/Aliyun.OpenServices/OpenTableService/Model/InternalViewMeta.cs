// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Model.InternalViewMeta
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
  [XmlRoot(ElementName = "View")]
  public class InternalViewMeta
  {
    public string Name { get; set; }

    public int PagingKeyLen { get; set; }

    [XmlElement(ElementName = "PrimaryKey")]
    public List<PrimaryKey> PrimaryKeys { get; set; }

    [XmlElement(ElementName = "Column")]
    public List<ViewColumn> Columns { get; set; }

    public InternalViewMeta()
    {
      this.PrimaryKeys = new List<PrimaryKey>();
      this.Columns = new List<ViewColumn>();
    }

    internal ViewMeta ToOpenTableViewMeta()
    {
      ViewMeta viewMeta = new ViewMeta(this.Name);
      viewMeta.PagingKeyLength = this.PagingKeyLen;
      foreach (PrimaryKey primaryKey in this.PrimaryKeys)
        viewMeta.PrimaryKeys.Add(primaryKey.Name, PrimaryKeyTypeHelper.Parse(primaryKey.PrimaryKeyType));
      foreach (ViewColumn viewColumn in this.Columns)
        viewMeta.AttributeColumns.Add(viewColumn.Name, ColumnTypeHelper.Parse(viewColumn.ColumnType));
      return viewMeta;
    }
  }
}
