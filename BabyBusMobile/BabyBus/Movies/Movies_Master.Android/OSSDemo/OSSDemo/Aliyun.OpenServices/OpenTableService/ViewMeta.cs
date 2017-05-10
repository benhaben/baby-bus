// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.ViewMeta
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示视图（View）的结构信息。
  /// 
  /// </summary>
  public class ViewMeta
  {
    private string _viewName;
    private EntityDictionary<PrimaryKeyType> _primaryKeys;
    private EntityDictionary<ColumnType> _columns;

    /// <summary>
    /// 获取或设置视图（View）名。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 名称用来表示表组（Table Group）、表（Table）、视图（View）、列（Column）等的名字，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
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
        if (string.IsNullOrEmpty(value) || !OtsUtility.IsEntityNameValid(value))
          throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");
        this._viewName = value;
      }
    }

    /// <summary>
    /// 获取主键列的名称与数据类型的对应字典。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// </para>
    /// 
    /// <para>
    /// 字典的值（Value）表示主键列的数据类型<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyType"/>。
    /// </para>
    /// 
    /// </remarks>
    public IDictionary<string, PrimaryKeyType> PrimaryKeys
    {
      get
      {
        return (IDictionary<string, PrimaryKeyType>) this._primaryKeys;
      }
    }

    /// <summary>
    /// 获取属性列的名称与数据类型的对应字典。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// </para>
    /// 
    /// <para>
    /// 字典的值（Value）表示主键列的数据类型<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnType"/>。
    /// </para>
    /// 
    /// </remarks>
    public IDictionary<string, ColumnType> AttributeColumns
    {
      get
      {
        return (IDictionary<string, ColumnType>) this._columns;
      }
    }

    /// <summary>
    /// 获取或设置一个值表示在当前视图上前几个主键列（Primary Key）建立分页（Paging）。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 此参数的值必须小于主键列的个数，如果设置为0表示不建立分页键。
    /// 
    /// </remarks>
    public int PagingKeyLength { get; set; }

    private ViewMeta()
    {
      this._primaryKeys = new EntityDictionary<PrimaryKeyType>();
      this._columns = new EntityDictionary<ColumnType>();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.ViewMeta"/>实例。
    /// 
    /// </summary>
    /// <param name="viewName">视图（View）名。</param>
    public ViewMeta(string viewName)
      : this()
    {
      if (string.IsNullOrEmpty(viewName) || !OtsUtility.IsEntityNameValid(viewName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "viewName");
      this.ViewName = viewName;
    }
  }
}
