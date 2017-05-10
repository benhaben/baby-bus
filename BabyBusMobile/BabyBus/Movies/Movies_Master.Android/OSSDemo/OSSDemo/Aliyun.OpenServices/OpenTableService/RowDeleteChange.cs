// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.RowDeleteChange
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示数据行的删除信息。
  /// 
  /// </summary>
  public class RowDeleteChange : RowChange
  {
    /// <summary>
    /// 获取变更方式。
    /// 
    /// </summary>
    internal override string ModifyType
    {
      get
      {
        return "DELETE";
      }
    }

    /// <summary>
    /// 获取要删除属性列（Attribute Column）的名称列表，如果该列表为空则删除整行。
    /// 
    /// </summary>
    public ICollection<string> ColumnNames { get; private set; }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>实例。
    /// 
    /// </summary>
    public RowDeleteChange()
    {
      this.InitializeColumns();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>实例。
    /// 
    /// </summary>
    /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
    public RowDeleteChange(IDictionary<string, PrimaryKeyValue> primaryKeys)
      : base(primaryKeys)
    {
      this.InitializeColumns();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>实例。
    /// 
    /// </summary>
    /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param><param name="columnNames">要删除属性列（Attribute Column）的名称列表。</param>
    public RowDeleteChange(IDictionary<string, PrimaryKeyValue> primaryKeys, IEnumerable<string> columnNames)
      : this(primaryKeys)
    {
      if (columnNames == null)
        throw new ArgumentNullException("columnNames");
      this.ColumnNames = (ICollection<string>) new EntityNameList(columnNames);
    }

    private void InitializeColumns()
    {
      this.ColumnNames = (ICollection<string>) new EntityNameList();
    }
  }
}
