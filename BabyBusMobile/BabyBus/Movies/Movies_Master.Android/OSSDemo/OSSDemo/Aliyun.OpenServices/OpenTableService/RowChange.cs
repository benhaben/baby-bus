// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.RowChange
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示行的数据变更信息。
  /// 
  /// </summary>
  public abstract class RowChange
  {
    /// <summary>
    /// 获取主键（Primary Key）列名称与值的对应字典。
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
    public IDictionary<string, PrimaryKeyValue> PrimaryKeys { get; private set; }

    /// <summary>
    /// 获取变更方式。
    /// 
    /// </summary>
    internal abstract string ModifyType { get; }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowChange"/>对象。
    /// 
    /// </summary>
    protected RowChange()
    {
      this.PrimaryKeys = (IDictionary<string, PrimaryKeyValue>) new PrimaryKeyDictionary();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowChange"/>对象。
    /// 
    /// </summary>
    /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
    protected RowChange(IDictionary<string, PrimaryKeyValue> primaryKeys)
      : this()
    {
      if (primaryKeys == null)
        throw new ArgumentNullException("primaryKeys");
      foreach (KeyValuePair<string, PrimaryKeyValue> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyValue>>) primaryKeys)
        this.PrimaryKeys.Add(keyValuePair);
    }
  }
}
