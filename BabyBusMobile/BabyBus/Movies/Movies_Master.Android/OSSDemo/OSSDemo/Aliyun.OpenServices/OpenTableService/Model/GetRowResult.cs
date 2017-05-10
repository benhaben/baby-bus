// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Model.GetRowResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
  public class GetRowResult : OpenTableServiceResult
  {
    [XmlArrayItem("Row")]
    [XmlArray("Table")]
    public List<InternalTableRow> Rows { get; set; }

    public Row GetSingleRow()
    {
      if (this.Rows == null || this.Rows.Count == 0 || this.Rows.Count == 1 && this.Rows[0].Columns.Count == 0)
        return (Row) null;
      Debug.Assert(this.Rows.Count == 1);
      return this.Rows[0].ToRow();
    }

    public IEnumerable<Row> GetMultipleRows()
    {
      return this.Rows != null ? Enumerable.Select<InternalTableRow, Row>((IEnumerable<InternalTableRow>) this.Rows, (Func<InternalTableRow, Row>) (r => r.ToRow())) : (IEnumerable<Row>) new List<Row>();
    }
  }
}
