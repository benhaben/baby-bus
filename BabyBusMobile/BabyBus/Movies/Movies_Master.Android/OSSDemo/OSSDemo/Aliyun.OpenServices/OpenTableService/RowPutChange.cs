// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.RowPutChange
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示行的插入或更新信息。
  /// 
  /// </summary>
  /// 
  /// <example>
  /// 下面的示例代码演示如何通过<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的方法插入一条数据。
  /// 
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.Linq;
  /// using Aliyun.OpenServices.OpenTableService;
  /// 
  /// namespace Aliyun.OpenServices.Samples.OpenTableService
  /// {
  ///     class PutDataSample
  ///     {
  ///         string endpoint = "http://ots.aliyuncs.com";
  ///         string accessId = "<your access id>";
  ///         string accessKey = "<your access key>";
  /// 
  ///         public void PutData(string tableName)
  ///         {
  ///             // 构造RowPutChange
  ///             var rowChange = new RowPutChange();
  ///             // 注意rowChange的主键信息必须与创建表时指定的主键个数、名称及类型均一致
  ///             // 可以直接赋值主键为支持的类型对象，包括整型、布尔型和字符串。
  ///             rowChange.PrimaryKeys["uid"] = 1;
  ///             rowChange.PrimaryKeys["flag"] = true;
  ///             rowChange.PrimaryKeys["name"] = "张三";
  ///             //.其他属性信息放在AttributeColumns中，可以是建表时没有指定的列
  ///             // 可以直接赋值列值为支持的类型对象，包括整型、浮点型、布尔型和字符串。
  ///             rowChange.AttributeColumns["groupid"] = 1;
  ///             rowChange.AttributeColumns["mobile"] = "11111111111";
  ///             rowChange.AttributeColumns["address"] = "中国某地";
  ///             rowChange.AttributeColumns["age"] = 20;
  /// 
  ///             // 提交数据
  ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
  /// 
  ///             try
  ///             {
  ///                 otsClient.PutData(tableName, rowChange);
  ///             }
  ///             catch (OtsException ex)
  ///             {
  ///                 Console.WriteLine("插入数据失败。OTS异常消息： " + ex.Message);
  ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
  ///             }
  ///             catch (System.Net.WebException ex)
  ///             {
  ///                 Console.WriteLine("创建表失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
  ///             }
  ///         }
  ///     }
  /// }
  ///         ]]>
  /// </code>
  /// 
  /// </example>
  public class RowPutChange : RowChange
  {
    /// <summary>
    /// 获取变更方式。
    /// 
    /// </summary>
    internal override string ModifyType
    {
      get
      {
        return "PUT";
      }
    }

    /// <summary>
    /// 获取或设置进行数据存在性检查的方式。
    /// 
    /// </summary>
    public CheckingMode CheckingMode { get; set; }

    /// <summary>
    /// 获取属性列（Attribute Column）名称与值的对应字典。
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
    public IDictionary<string, ColumnValue> AttributeColumns { get; private set; }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>实例。
    /// 
    /// </summary>
    public RowPutChange()
    {
      this.AttributeColumns = (IDictionary<string, ColumnValue>) new EntityDictionary<ColumnValue>();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>实例。
    /// 
    /// </summary>
    /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
    public RowPutChange(IDictionary<string, PrimaryKeyValue> primaryKeys)
      : base(primaryKeys)
    {
      this.AttributeColumns = (IDictionary<string, ColumnValue>) new EntityDictionary<ColumnValue>();
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>实例。
    /// 
    /// </summary>
    /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param><param name="columns">属性列（Attribute Column）名称与值的对应字典。</param>
    public RowPutChange(IDictionary<string, PrimaryKeyValue> primaryKeys, IDictionary<string, ColumnValue> columns)
      : base(primaryKeys)
    {
      if (columns == null)
        throw new ArgumentNullException("columns");
      this.AttributeColumns = (IDictionary<string, ColumnValue>) new EntityDictionary<ColumnValue>(columns);
    }
  }
}
