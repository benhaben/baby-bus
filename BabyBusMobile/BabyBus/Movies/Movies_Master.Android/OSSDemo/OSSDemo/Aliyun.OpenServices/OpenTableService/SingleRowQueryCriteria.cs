// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示获取一行数据的查询条件。
  /// 
  /// </summary>
  public class SingleRowQueryCriteria : RowQueryCriteria
  {
    private PrimaryKeyDictionary _primaryKeys = new PrimaryKeyDictionary();

    /// <summary>
    /// 获取主键（Primary Key）名称与值的对应字典。
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
    /// 字典的值（Value）表示主键列的值<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>。
    /// </para>
    /// 
    /// </remarks>
    public IDictionary<string, PrimaryKeyValue> PrimaryKeys
    {
      get
      {
        return (IDictionary<string, PrimaryKeyValue>) this._primaryKeys;
      }
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>实例。
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
    public SingleRowQueryCriteria(string tableName)
      : this(tableName, (string) null)
    {
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">要查询的表（Table）名。</param><param name="viewName">要查询的视图（View）名。</param><exception cref="T:System.ArgumentException">
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
    public SingleRowQueryCriteria(string tableName, string viewName)
      : this(tableName, viewName, (IDictionary<string, PrimaryKeyValue>) null, (IEnumerable<string>) null)
    {
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">要查询的表（Table）名。</param><param name="primaryKeys">主键（Primary Key）名称与值的对应字典。</param><param name="columnNames">需要返回列（Column）的名称的列表。</param><exception cref="T:System.ArgumentException">
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
    /// </exception><exception cref="T:System.ArgumentNullException">primaryKeys为空引用。</exception>
    public SingleRowQueryCriteria(string tableName, IDictionary<string, PrimaryKeyValue> primaryKeys, IEnumerable<string> columnNames)
      : this(tableName, (string) null, primaryKeys, columnNames)
    {
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>实例。
    /// 
    /// </summary>
    /// <param name="tableName">要查询的表（Table）名。</param><param name="viewName">要查询的视图（View）名。</param><param name="primaryKeys">主键（Primary Key）名称与值的对应字典。</param><param name="columnNames">需要返回列（Column）的名称的列表。</param><exception cref="T:System.ArgumentException">
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
    public SingleRowQueryCriteria(string tableName, string viewName, IDictionary<string, PrimaryKeyValue> primaryKeys, IEnumerable<string> columnNames)
      : base(tableName, viewName, columnNames)
    {
      if (primaryKeys == null)
        return;
      foreach (KeyValuePair<string, PrimaryKeyValue> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyValue>>) primaryKeys)
        this.PrimaryKeys.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }
}
