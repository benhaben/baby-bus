// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Row
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示行。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// 表示查询时返回的数据行。<see cref="P:Aliyun.OpenServices.OpenTableService.Row.Columns"/>包含了查询时指定的返回列，可能有主键列也可能有属性列。
  ///             如果查询时没有指定返回列，则包含有整行所有列的数据。
  /// </para>
  /// 
  /// <para>
  /// 同时也可以使用该对象的枚举器枚举所有列的名称和值的对。
  /// </para>
  /// 
  /// </remarks>
  public class Row : IEnumerable<KeyValuePair<string, ColumnValue>>, IEnumerable
  {
    private EntityDictionary<ColumnValue> _columns = new EntityDictionary<ColumnValue>();

    /// <summary>
    /// 获取列（Column）名称与值的对应字典。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 字典的键（Key）表示列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// </para>
    /// 
    /// <para>
    /// 字典的值（Value）表示列的值<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>。
    /// </para>
    /// 
    /// </remarks>
    public IDictionary<string, ColumnValue> Columns
    {
      get
      {
        return (IDictionary<string, ColumnValue>) this._columns;
      }
    }

    public IEnumerator<KeyValuePair<string, ColumnValue>> GetEnumerator()
    {
      return this.Columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
