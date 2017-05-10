// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.RowQueryCriteria
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示获取行的查询条件。
  /// 
  /// </summary>
  public abstract class RowQueryCriteria
  {
    private IList<string> _columns = (IList<string>) new EntityNameList();
    private string _tableName;
    private string _viewName;

    /// <summary>
    /// 获取或设置要查询的表（Table）名。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 名称由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// 
    /// </remarks>
    public string TableName
    {
      get
      {
        return this._tableName;
      }
      set
      {
        if (string.IsNullOrEmpty(value) || !OtsUtility.IsEntityNameValid(value))
          throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");
        this._tableName = value;
      }
    }

    /// <summary>
    /// 获取或设置要查询的视图（View）名。如果该属性未设置或设置为空引用或空字符串，则从表中查询。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 名称由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// 
    /// </remarks>
    public string ViewName
    {
      get
      {
        return this._viewName;
      }
      set
      {
        if (!OtsUtility.IsEntityNameValid(value))
          throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");
        this._viewName = value;
      }
    }

    /// <summary>
    /// 获取需要返回列（Column）的名称的列表。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 列的名称由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// </para>
    /// 
    /// <para>
    /// 如果列表中指定了要返回的列的名称，则返回的结果中将只包含指定列的数据。否则，如果列表为空，则返回结果将包含行中的所有列的数据。
    /// </para>
    /// 
    /// </remarks>
    public IList<string> ColumnNames
    {
      get
      {
        return this._columns;
      }
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">要查询的表（Table）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableName不符合OTS名称的命名规范。
    /// </para>
    /// </exception>
    protected RowQueryCriteria(string tableName)
      : this(tableName, (string) null)
    {
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param><param name="viewName">要查询的视图（View）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableName不符合OTS名称的命名规范。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。
    /// </para>
    /// </exception>
    protected RowQueryCriteria(string tableName, string viewName)
      : this(tableName, viewName, (IEnumerable<string>) null)
    {
      this._tableName = tableName;
      this._viewName = viewName;
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param><param name="columnNames">需要返回列（Column）的名称的列表。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableName不符合OTS名称的命名规范。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">columnNames为空引用。</exception>
    protected RowQueryCriteria(string tableName, IEnumerable<string> columnNames)
      : this(tableName, (string) null, columnNames)
    {
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param><param name="viewName">要查询的视图（View）名。</param><param name="columnNames">需要返回列（Column）的名称的列表。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableName不符合OTS名称的命名规范。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。
    /// </para>
    /// </exception>
    protected RowQueryCriteria(string tableName, string viewName, IEnumerable<string> columnNames)
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      if (!string.IsNullOrEmpty(viewName) && !OtsUtility.IsEntityNameValid(viewName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "viewName");
      this._tableName = tableName;
      this._viewName = viewName;
      if (columnNames == null)
        return;
      foreach (string str in columnNames)
        this.ColumnNames.Add(str);
    }
  }
}
