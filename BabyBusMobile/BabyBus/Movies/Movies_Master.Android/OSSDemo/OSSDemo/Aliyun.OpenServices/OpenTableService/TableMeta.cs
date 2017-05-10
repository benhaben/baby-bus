// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.TableMeta
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示表（Table）的结构信息。
  /// 
  /// </summary>
  public class TableMeta
  {
    private string _tableName;
    private string _tableGroupName;
    private EntityDictionary<PrimaryKeyType> _primaryKeys;

    /// <summary>
    /// 获取或设置表（Table）名。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 名称用来表示表组（Table Group）、表（Table）、视图（View）、列（Column）等的名字，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
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
    /// 获取或设置表组（Table Group）名。
    ///             如果该属性未设置或设置为空引用或值为空字符串，则该表不创建在表组中。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 名称用来表示表组（Table Group）、表（Table）、视图（View）、列（Column）等的名字，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    /// 
    /// </remarks>
    public string TableGroupName
    {
      get
      {
        return this._tableGroupName;
      }
      set
      {
        if (!string.IsNullOrEmpty(value) && !OtsUtility.IsEntityNameValid(value))
          throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");
        this._tableGroupName = value;
      }
    }

    /// <summary>
    /// 获取或设置一个值，表示分页建立在前几个主键（Primary Key）列上。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 此参数的值必须小于主键列的个数，如果未设置或设置为0表示不建立分页键。
    /// </remarks>
    public int PagingKeyLength { get; set; }

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
    /// 获取与该表（Table）相关的视图（View）的列表。
    /// 
    /// </summary>
    public IList<ViewMeta> Views { get; private set; }

    private TableMeta()
    {
      this._primaryKeys = new EntityDictionary<PrimaryKeyType>();
      this.Views = (IList<ViewMeta>) new List<ViewMeta>();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.TableMeta"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名。</param><exception cref="T:System.ArgumentException">名称用来表示表组（Table Group）、表（Table）、视图（View）、列（Column）等的名字，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    ///     </exception>
    public TableMeta(string tableName)
      : this()
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      this.TableName = tableName;
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.TableMeta"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名。</param><param name="primaryKeys">主键列名称与数据类型的对应字典。</param><exception cref="T:System.ArgumentException">名称用来表示表组（Table Group）、表（Table）、视图（View）、列（Column）等的名字，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
    ///     </exception><exception cref="T:System.ArgumentNullException">primaryKeys为空引用。</exception>
    public TableMeta(string tableName, IDictionary<string, PrimaryKeyType> primaryKeys)
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      if (primaryKeys == null)
        throw new ArgumentNullException("primaryKeys");
      this.TableName = tableName;
      this._primaryKeys = new EntityDictionary<PrimaryKeyType>(primaryKeys);
      this.Views = (IList<ViewMeta>) new List<ViewMeta>();
    }
  }
}
