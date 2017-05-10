// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Model.InternalTableRow
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
  [XmlRoot("Row")]
  public class InternalTableRow
  {
    [XmlElement("Column")]
    public List<InternalTableColumn> Columns { get; set; }

    public Row ToRow()
    {
      Row row = new Row();
      foreach (InternalTableColumn internalTableColumn in this.Columns)
      {
        string @string;
        if (!string.IsNullOrEmpty(internalTableColumn.Value.Encoding))
        {
          switch (internalTableColumn.Value.Encoding)
          {
            case "Base64":
              byte[] bytes = Convert.FromBase64String(internalTableColumn.Value.Value);
              try
              {
                @string = OtsUtility.DataEncoding.GetString(bytes);
                break;
              }
              catch (DecoderFallbackException ex)
              {
                throw ExceptionFactory.CreateInvalidResponseException((ServiceResponse) null, OtsExceptions.ColumnValueCannotBeDecoded, (Exception) null);
              }
            default:
              throw ExceptionFactory.CreateInvalidResponseException((ServiceResponse) null, string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.UnsupportedEncodingFormat, new object[1]
              {
                (object) internalTableColumn.Value.Encoding
              }), (Exception) null);
          }
        }
        else
          @string = internalTableColumn.Value.Value;
        row.Columns[internalTableColumn.Name] = new ColumnValue(@string, ColumnTypeHelper.Parse(internalTableColumn.Value.ColumnType));
      }
      return row;
    }
  }
}
