// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.IOts
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示用于访问阿里云开放结构化数据（Open Table Service，简称OTS）的公共方法。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// 开放结构化数据服务（Open Table Service，OTS）是构建在阿里云大规模分布式计算系统之上的海量数据存储与实时查询的服务。
  /// </para>
  /// 
  /// </remarks>
  /// 
  /// <example>
  /// 下面的示例代码演示如何通过<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的方法在OTS中创建一个表（Table）。
  /// 
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.Linq;
  /// using Aliyun.OpenTableService;
  /// 
  /// namespace Aliyun.OpenServices.Samples.OpenTableService
  /// {
  ///     class CreateTableSample
  ///     {
  ///         string endpoint = "http://ots.aliyuncs.com";
  ///         string accessId = "<your access id>";
  ///         string accessKey = "<your access key>";
  /// 
  ///         string tableName = "contact_table";
  ///         string viewName = "view1";
  /// 
  ///         public void CreateTable()
  ///         {
  ///             // 创建表结构信息。
  ///             var tableMeta = new TableMeta(tableName);
  ///             // 指定表的主键。
  ///             tableMeta.PrimaryKeys.Add("uid", PrimaryKeyType.Integer);
  ///             tableMeta.PrimaryKeys.Add("flag", PrimaryKeyType.Boolean);
  ///             tableMeta.PrimaryKeys.Add("name", PrimaryKeyType.String);
  ///             tableMeta.PagingKeyLength = 2;
  /// 
  ///             var viewMeta = new ViewMeta(viewName);
  ///             viewMeta.PrimaryKeys.Add("uid", PrimaryKeyType.Integer);
  ///             viewMeta.PrimaryKeys.Add("flag", PrimaryKeyType.Boolean);
  ///             tableMeta.PrimaryKeys.Add("name", PrimaryKeyType.String);
  ///             viewMeta.PrimaryKeys.Add("groupid", PrimaryKeyType.Integer);
  /// 
  ///             tableMeta.Views.Add(viewMeta);
  /// 
  ///             // 在OTS中创建一个表。
  ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
  /// 
  ///             try
  ///             {
  ///                 otsClient.CreateTable(tableMeta);
  ///             }
  ///             catch (OtsException ex)
  ///             {
  ///                 Console.WriteLine("创建表失败。OTS异常消息：" + ex.Message);
  ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常。
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
  public interface IOts
  {
    /// <summary>
    /// 创建表组（Table Group）。
    /// 
    /// </summary>
    /// <param name="tableGroupName">表组（Table Group）名称。</param><param name="partitionKeyType">表示表组（Table Group)的数据分片键（Partition Key）的类型。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableGroupName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 值违反了OTS名称的命名规则。
    /// </para>
    /// </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void CreateTableGroup(string tableGroupName, PartitionKeyType partitionKeyType);

    /// <summary>
    /// 开始创建表组（Table Group）的异步请求。
    /// 
    /// </summary>
    /// <param name="tableGroupName">表组（Table Group）名称。</param><param name="partitionKeyType">表示表组（Table Group)的数据分片键（Partition Key）的类型。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableGroupName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 值违反了OTS名称的命名规则。
    /// </para>
    /// </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginCreateTableGroup(string tableGroupName, PartitionKeyType partitionKeyType, AsyncCallback callback, object state);

    /// <summary>
    /// 结束创建表组（Table Group）的异步请求。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndCreateTableGroup(IAsyncResult asyncResult);

    /// <summary>
    /// 获取表组（Table Group）名的列表。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 表组（Table Group）名称的枚举器。
    /// </returns>
    /// <exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<string> ListTableGroups();

    /// <summary>
    /// 开始获取表组（Table Group）名列表的异步操作。
    /// 
    /// </summary>
    /// <param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginListTableGroups(AsyncCallback callback, object state);

    /// <summary>
    /// 结束获取表组（Table Group）列表的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<string> EndListTableGroups(IAsyncResult asyncResult);

    /// <summary>
    /// 删除表组（Table Group）及属于该表组的相关表（Table）和视图（View）。
    /// 
    /// </summary>
    /// <param name="tableGroupName">表组（Table Group）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableGroupName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 值违反了OTS名称的命名规则。
    /// </para>
    /// </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void DeleteTableGroup(string tableGroupName);

    /// <summary>
    /// 开始删除表组（Table Group）及属于该表组的相关表（Table）和视图（View）的异步操作。
    /// 
    /// </summary>
    /// <param name="tableGroupName">表组（Table Group）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableGroupName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 值违反了OTS名称的命名规则。
    /// </para>
    /// </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginDeleteTableGroup(string tableGroupName, AsyncCallback callback, object state);

    /// <summary>
    /// 结束删除表组（Table Group）及属于该表组的相关表（Table）和视图（View）的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndDeleteTableGroup(IAsyncResult asyncResult);

    /// <summary>
    /// 创建表（Table）及其相关视图（View）。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 这一操作在OTS中提交一个创建表的请求，
    ///         但该操作成功并不代表数据表已经创建完成。请在表创建完成后再对表进行进一步的操作，比如插入数据等。
    ///         如需检查表是否创建完成，请使用ListTables方法检查表是否存在。
    /// 
    /// </para>
    /// 
    /// <para>
    /// OTS要求视图中数据行与原表数据行一一对应，因此强烈建议视图的主键（Primary Key）列包含表的全部主键列。
    /// 
    /// </para>
    /// 
    /// </remarks>
    /// <param name="tableMeta"><see cref="T:Aliyun.OpenServices.OpenTableService.TableMeta"/>的对象，包含表（Table）及视图（View）的结构信息。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableMeta为空引用，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableMeta中的结构信息无效。
    /// </para>
    /// </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    /// <example>
    /// 下面的示例代码演示如何通过<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的方法在OTS中创建一个表（Table）。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using Aliyun.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class CreateTableSample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    /// 
    ///         string tableName = "contact_table";
    ///         string viewName = "view1";
    /// 
    ///         public void CreateTable()
    ///         {
    ///             // 创建表结构信息。
    ///             var tableMeta = new TableMeta(tableName);
    ///             // 指定表的主键。
    ///             tableMeta.PrimaryKeys.Add("uid", PrimaryKeyType.Integer);
    ///             tableMeta.PrimaryKeys.Add("flag", PrimaryKeyType.Boolean);
    ///             tableMeta.PrimaryKeys.Add("name", PrimaryKeyType.String);
    ///             tableMeta.PagingKeyLength = 2;
    /// 
    ///             var viewMeta = new ViewMeta(viewName);
    ///             viewMeta.PrimaryKeys.Add("uid", PrimaryKeyType.Integer);
    ///             viewMeta.PrimaryKeys.Add("flag", PrimaryKeyType.Boolean);
    ///             tableMeta.PrimaryKeys.Add("name", PrimaryKeyType.String);
    ///             viewMeta.PrimaryKeys.Add("groupid", PrimaryKeyType.Integer);
    /// 
    ///             tableMeta.Views.Add(viewMeta);
    /// 
    ///             // 在OTS中创建一个表。
    ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
    /// 
    ///             try
    ///             {
    ///                 otsClient.CreateTable(tableMeta);
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("创建表失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常。
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
    void CreateTable(TableMeta tableMeta);

    /// <summary>
    /// 开始创建表（Table）及其相关视图（View）的异步操作。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 这一操作在OTS中提交一个创建表的请求，
    ///         但该操作成功并不代表数据表已经创建完成。请在表创建完成后再对表进行进一步的操作，比如插入数据等。
    ///         如需检查表是否创建完成，请使用ListTables方法检查表是否存在。
    /// 
    /// </para>
    /// 
    /// <para>
    /// OTS要求视图中数据行与原表数据行一一对应，因此强烈建议视图的主键（Primary Key）列包含表的全部主键列。
    /// 
    /// </para>
    /// 
    /// </remarks>
    /// <param name="tableMeta"><see cref="T:Aliyun.OpenServices.OpenTableService.TableMeta"/>的对象，包含表（Table）及视图（View）的结构信息。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableMeta为空引用，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableMeta中的结构信息无效。
    /// </para>
    /// </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginCreateTable(TableMeta tableMeta, AsyncCallback callback, object state);

    /// <summary>
    /// 结束创建表（Table）及其相关视图（View）的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndCreateTable(IAsyncResult asyncResult);

    /// <summary>
    /// 获取表（Table）的结构信息。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名。</param>
    /// <returns>
    /// 表示表（Table）的结构信息的<see cref="T:Aliyun.OpenServices.OpenTableService.TableMeta"/>对象。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">
    /// <para>
    /// tableMeta为空引用，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableMeta中的结构信息无效。
    /// </para>
    /// </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    TableMeta GetTableMeta(string tableName);

    /// <summary>
    /// 开始获取表（Table）的结构信息的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableMeta为空引用，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// tableMeta中的结构信息无效。
    /// </para>
    /// </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginGetTableMeta(string tableName, AsyncCallback callback, object state);

    /// <summary>
    /// 结束获取表（Table）的结构信息的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    TableMeta EndGetTableMeta(IAsyncResult asyncResult);

    /// <summary>
    /// 获取表（Table）名的列表。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 表（Table）名的枚举器。
    /// </returns>
    /// <exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<string> ListTables();

    /// <summary>
    /// 开始获取表（Table）名列表的异步操作。
    /// 
    /// </summary>
    /// <param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginListTables(AsyncCallback callback, object state);

    /// <summary>
    /// 结束获取表（Table）名列表的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<string> EndListTables(IAsyncResult asyncResult);

    /// <summary>
    /// 删除表（Table）及与此表一起创建的视图（View）。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 违反OTS名称的命名规则。
    /// </para>
    /// </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void DeleteTable(string tableName);

    /// <summary>
    /// 开始删除表（Table）及与此表一起创建的视图（View）的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 违反OTS名称的命名规则。
    /// </para>
    /// </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginDeleteTable(string tableName, AsyncCallback callback, object state);

    /// <summary>
    /// 结束删除表（Table）及与此表一起创建的视图（View）的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndDeleteTable(IAsyncResult asyncResult);

    /// <summary>
    /// 在表或表组上开始一个事务（Transaction），并得到该事务ID。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 用户必须指定数据分片键，并保证所有在这个事务中的操作的数据分片键的值等于在此API中指定的数据分片键的值。
    /// </para>
    /// 
    /// <para>
    /// 若此数据分片键已经存在另一个事务中且该事务没有完成或被取消，则本次事务会直接失败，用户可以重试。
    /// </para>
    /// 
    /// </remarks>
    /// <param name="entityName">表（Table）名或表组（Table Group）名。</param><param name="partitionKeyValue">表示事务（Transaction）建立在哪个数据分片键（Partition Key）上。</param>
    /// <returns>
    /// 事务（Transaction）ID。
    /// </returns>
    /// <exception cref="T:System.ArgumentException">
    /// <para>
    /// entityName为空引用或空字符串
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// entityName违反OTS名称的命名规范。
    /// </para>
    /// </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    /// <example>
    /// 下面的示例代码演示如何开始一个事务（Transaction），并异步地批量提交数据修改。该示例中使用<see cref="M:Aliyun.OpenServices.OpenTableService.OtsClient.CreateTable(Aliyun.OpenServices.OpenTableService.TableMeta)"/>方法示例代码所创建的表。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using System.Net;
    /// using Aliyun.OpenServices.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class AsyncBatchModifyDataSample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    ///         OtsClient otsClient;
    /// 
    ///         public AsyncBatchModifyDataSample()
    ///         {
    ///             otsClient = new OtsClient(endpoint, accessId, accessKey);
    ///         }
    /// 
    ///         public void BatchInsertData(string tableName)
    ///         {
    ///             int uid = 1;
    ///             // 将5条RowPutChange放到列表中
    ///             var rowChanges = new List<RowChange>();
    /// 
    ///             for (int i = 0; i < 5; i++)
    ///             {
    ///                 var rowChange = new RowPutChange();
    ///                 rowChange.PrimaryKeys["uid"] = uid;
    ///                 rowChange.PrimaryKeys["flag"] = true;
    ///                 rowChange.PrimaryKeys["name"] = "contact " + i.ToString();
    /// 
    ///                 rowChange.AttributeColumns["groupid"] = 1;
    ///                 rowChange.AttributeColumns["address"] = "中国某地";
    /// 
    ///                 rowChanges.Add(rowChange);
    ///             }
    /// 
    ///             // 开始一个事务
    ///             try
    ///             {
    ///                 var transactionId = otsClient.StartTransaction(tableName, uid);
    /// 
    ///                 // 开始批量修改数据的异步操作
    ///                 otsClient.BeginBatchModifyData(tableName, rowChanges, transactionId, BatchModifyDataCallback, transactionId);
    /// 
    ///                 // 这里进行其他操作
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    /// 
    ///         private void BatchModifyDataCallback(IAsyncResult asyncResult)
    ///         {
    ///             try
    ///             {
    ///                 var transactionId = asyncResult.AsyncState as string;
    /// 
    ///                 // 操作结束时必须调用EndBatchModifyData方法，否则可能会产生资源泄露
    ///                 otsClient.EndBatchModifyData(asyncResult);
    /// 
    ///                 // 需要调用ComitTransaction方法提交数据修改
    ///                 otsClient.CommitTransaction(transactionId);
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    ///     }
    /// }
    ///         ]]>
    /// </code>
    /// 
    /// </example>
    string StartTransaction(string entityName, PartitionKeyValue partitionKeyValue);

    /// <summary>
    /// 开始一个异步操作：在表或表组上开始一个事务（Transaction），并得到该事务ID。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 用户必须指定数据分片键，并保证所有在这个事务中的操作的数据分片键的值等于在此API中指定的数据分片键的值。
    /// </para>
    /// 
    /// <para>
    /// 若此数据分片键已经存在另一个事务中且该事务没有完成或被取消，则本次事务会直接失败，用户可以重试。
    /// </para>
    /// 
    /// </remarks>
    /// <param name="entityName">表（Table）名或表组（Table Group）名。</param><param name="partitionKeyValue">表示事务（Transaction）建立在哪个数据分片键（Partition Key）上。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// entityName为空引用或空字符串
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// entityName违反OTS名称的命名规范。
    /// </para>
    /// </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginStartTransaction(string entityName, PartitionKeyValue partitionKeyValue, AsyncCallback callback, object state);

    /// <summary>
    /// 结束在表或表组上开始一个事务（Transaction），并得到该事务ID的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    string EndStartTransaction(IAsyncResult asyncResult);

    /// <summary>
    /// 确认并提交事务（Transaction）。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 确认并提交事务（Transaction），提交后此事务ID失效。
    /// </remarks>
    /// <param name="transactionId">事务（Transaction）ID。</param><exception cref="T:System.ArgumentException">transactionId为空引用或值为空字符串。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    /// <example>
    /// 下面的示例代码演示如何开始一个事务（Transaction），并异步地批量提交数据修改。该示例中使用<see cref="M:Aliyun.OpenServices.OpenTableService.OtsClient.CreateTable(Aliyun.OpenServices.OpenTableService.TableMeta)"/>方法示例代码所创建的表。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using System.Net;
    /// using Aliyun.OpenServices.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class AsyncBatchModifyDataSample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    ///         OtsClient otsClient;
    /// 
    ///         public AsyncBatchModifyDataSample()
    ///         {
    ///             otsClient = new OtsClient(endpoint, accessId, accessKey);
    ///         }
    /// 
    ///         public void BatchInsertData(string tableName)
    ///         {
    ///             int uid = 1;
    ///             // 将5条RowPutChange放到列表中
    ///             var rowChanges = new List<RowChange>();
    /// 
    ///             for (int i = 0; i < 5; i++)
    ///             {
    ///                 var rowChange = new RowPutChange();
    ///                 rowChange.PrimaryKeys["uid"] = uid;
    ///                 rowChange.PrimaryKeys["flag"] = true;
    ///                 rowChange.PrimaryKeys["name"] = "contact " + i.ToString();
    /// 
    ///                 rowChange.AttributeColumns["groupid"] = 1;
    ///                 rowChange.AttributeColumns["address"] = "中国某地";
    /// 
    ///                 rowChanges.Add(rowChange);
    ///             }
    /// 
    ///             // 开始一个事务
    ///             try
    ///             {
    ///                 var transactionId = otsClient.StartTransaction(tableName, uid);
    /// 
    ///                 // 开始批量修改数据的异步操作
    ///                 otsClient.BeginBatchModifyData(tableName, rowChanges, transactionId, BatchModifyDataCallback, transactionId);
    /// 
    ///                 // 这里进行其他操作
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    /// 
    ///         private void BatchModifyDataCallback(IAsyncResult asyncResult)
    ///         {
    ///             try
    ///             {
    ///                 var transactionId = asyncResult.AsyncState as string;
    /// 
    ///                 // 操作结束时必须调用EndBatchModifyData方法，否则可能会产生资源泄露
    ///                 otsClient.EndBatchModifyData(asyncResult);
    /// 
    ///                 // 需要调用ComitTransaction方法提交数据修改
    ///                 otsClient.CommitTransaction(transactionId);
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    ///     }
    /// }
    ///         ]]>
    /// </code>
    /// 
    /// </example>
    void CommitTransaction(string transactionId);

    /// <summary>
    /// 开始确认并提交事务（Transaction）的异步操作。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 确认并提交事务（Transaction），提交后此事务ID失效。
    /// </remarks>
    /// <param name="transactionId">事务（Transaction）ID。</param><exception cref="T:System.ArgumentException">transactionId为空引用或值为空字符串。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginCommitTransaction(string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束确认并提交事务（Transaction）的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndCommitTransaction(IAsyncResult asyncResult);

    /// <summary>
    /// 撤销一个事务（Transaction）。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 撤销一个事务（Transaction），撤销后所有在此事务中的操作都被取消，撤销后此事务ID失效。
    /// 
    /// </remarks>
    /// <param name="transactionId">事务（Transaction）ID。</param><exception cref="T:System.ArgumentException">transactionId为空引用或值为空字符串。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void AbortTransaction(string transactionId);

    /// <summary>
    /// 开始撤销一个事务（Transaction）的异步操作。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 撤销一个事务（Transaction），撤销后所有在此事务中的操作都被取消，撤销后此事务ID失效。
    /// 
    /// </remarks>
    /// <param name="transactionId">事务（Transaction）ID。</param><exception cref="T:System.ArgumentException">transactionId为空引用或值为空字符串。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginAbortTransaction(string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束撤销一个事务（Transaction）的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndAbortTransaction(IAsyncResult asyncResult);

    /// <summary>
    /// 获取表（Table）或视图（View）中的一行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>对象。
    ///     </param>
    /// <returns>
    /// 表示一行数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>的对象。如果指定的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象没找到，则返回null。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">criteria包含的信息无效。
    ///     </exception><exception cref="T:System.ArgumentNullException">criteria为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    /// <example>
    /// 下面的示例代码获取指定员工信息并根据基础工资和奖金比率计算奖金。示例中使用了<see cref="M:Aliyun.OpenServices.OpenTableService.OtsClient.GetRow(Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria)"/>方法，并进行了类型转换。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using Aliyun.OpenServices.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class ValueConversionSample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    /// 
    ///         public void ComputeBonus(string tableName, int uid, string employee)
    ///         {
    ///             // 构造查询条件，假定数据的主键是uid和name
    ///             var criteria = new SingleRowQueryCriteria(tableName);
    ///             criteria.PrimaryKeys["uid"] = uid;
    ///             criteria.PrimaryKeys["name"] = employee;
    ///             // 指定返回列，假定rate列是比率，类型为Double；salary列为基本工资，类型为Integer
    ///             criteria.ColumnNames.Add("name");
    ///             criteria.ColumnNames.Add("rate");
    ///             criteria.ColumnNames.Add("salary");
    /// 
    ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
    /// 
    ///             try
    ///             {
    ///                 // 读取数据
    ///                 var row = otsClient.GetRow(criteria);
    ///                 if (row == null)
    ///                 {
    ///                     Console.WriteLine("未找到员工{0}的信息。", employee);
    ///                 }
    /// 
    ///                 try
    ///                 {
    ///                     // 显式转换rate列的值为double类型，如果rate的ValueType != ColumnType.Double，则会抛出异常
    ///                     double rate = (double)row.Columns["rate"];
    ///                     // 显式转换rate列的值为long类型, 如果salary列的ValueType != ColumnType.Integer，则会抛出异常
    ///                     long salary = (long)row.Columns["salary"];
    /// 
    ///                     var bonus = rate * salary;
    ///                     Console.WriteLine("奖金数：{0}", bonus);
    ///                 }
    ///                 catch (InvalidCastException)
    ///                 {
    ///                     Console.WriteLine("存储数据格式错误。");
    ///                 }
    ///             }
    ///             catch (OpenTableServiceException ex)
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
    Row GetRow(SingleRowQueryCriteria criteria);

    /// <summary>
    /// 获取表（Table）或视图（View）中的一行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>对象。
    ///     </param>
    /// <returns>
    /// 表示一行数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>的对象。如果指定的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象没找到，则返回null。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">criteria包含的信息无效。
    ///     </exception><exception cref="T:System.ArgumentNullException">criteria为空引用。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    Row GetRow(SingleRowQueryCriteria criteria, string transactionId);

    /// <summary>
    /// 开始获取表（Table）或视图（View）中的一行数据的异步操作。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">criteria包含的信息无效。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria, AsyncCallback callback, object state);

    /// <summary>
    /// 开始获取表（Table）或视图（View）中的一行数据的异步操作。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">criteria包含的信息无效。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria, string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束获取表（Table）或视图（View）中的一行数据的异步操作。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 表示一行数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>的对象。如果指定的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象没找到，则返回null。
    /// 
    /// </returns>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    Row EndGetRow(IAsyncResult asyncResult);

    /// <summary>
    /// 获取表（Table）或视图（View）中主键（Primary Key）的特定范围内的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.RangeRowQueryCriteria"/>对象。
    ///     </param>
    /// <returns>
    /// 包含表数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象的枚举器。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">criteria包含的信息无效。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    /// <example>
    /// 下面的示例代码演示如何获取表（Table）中主键（Primary Key）的特定范围内的多行数据。该示例中使用了<see cref="M:Aliyun.OpenServices.OpenTableService.OtsClient.BeginBatchModifyData(System.String,System.Collections.Generic.IEnumerable{Aliyun.OpenServices.OpenTableService.RowChange},System.String,System.AsyncCallback,System.Object)"/>方法的示例代码所插入的数据。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using Aliyun.OpenServices.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class QuerySample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    /// 
    ///         public void GetRowsByRangeSample(string tableName)
    ///         {
    ///             // 构造查询条件，在tableName指定的表中进行查询。
    ///             var criteria = new RangeRowQueryCriteria(tableName);
    ///             // 设置主键值和主键范围，查询uid为1，flag为true，name值大于'contact 2'的多行数据。
    ///             criteria.PrimaryKeys["uid"] = 1;
    ///             criteria.PrimaryKeys["flag"] = true;
    ///             criteria.Range = new PrimaryKeyRange("name", "contact 2", PrimaryKeyRange.InfMax);
    ///             // 指定需要返回的列
    ///             criteria.ColumnNames.Add("groupid");
    ///             criteria.ColumnNames.Add("address");
    /// 
    ///             // 构造OtsClient对象
    ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
    /// 
    ///             // 调用查询方法
    ///             try
    ///             {
    ///                 var rows = otsClient.GetRowsByRange(criteria);
    ///                 foreach (var row in rows)
    ///                 {
    ///                     Console.WriteLine("Name: {0}", row.Columns["name"]);
    ///                     Console.WriteLine("Group ID: {0}", row.Columns["groupid"]);
    ///                     Console.WriteLine("Address: {0}", row.Columns["address"]);
    ///                     Console.WriteLine();
    ///                 }
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (System.Net.WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    ///     }
    /// }
    ///         ]]>
    /// </code>
    /// 
    /// </example>
    IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria);

    /// <summary>
    /// 获取表（Table）或视图（View）中主键（Primary Key）的特定范围内的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.RangeRowQueryCriteria"/>对象。
    ///     </param>
    /// <returns>
    /// 包含表数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象的枚举器。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">criteria包含的信息无效。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria, string transactionId);

    /// <summary>
    /// 开始异步操作获取表（Table）或视图（View）中主键（Primary Key）的特定范围内的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.RangeRowQueryCriteria"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">criteria包含的信息无效。
    ///     </exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria, AsyncCallback callback, object state);

    /// <summary>
    /// 开始异步操作获取表（Table）或视图（View）中主键（Primary Key）的特定范围内的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.RangeRowQueryCriteria"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">criteria包含的信息无效。
    ///     </exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria, string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束获取表（Table）或视图（View）中的一行数据的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<Row> EndGetRowsByRange(IAsyncResult asyncResult);

    /// <summary>
    /// 获取表（Table）或视图（View）的指定偏移量开始的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.OffsetRowQueryCriteria"/>对象。
    ///     </param>
    /// <returns>
    /// 包含表数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象的枚举器。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">criteria包含的信息无效。
    ///     </exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<Row> GetRowsByOffset(OffsetRowQueryCriteria criteria);

    /// <summary>
    /// 获取表（Table）或视图（View）的指定偏移量开始的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.OffsetRowQueryCriteria"/>对象。
    ///     </param>
    /// <returns>
    /// 包含表数据的<see cref="T:Aliyun.OpenServices.OpenTableService.Row"/>对象的枚举器。
    /// 
    /// </returns>
    /// <exception cref="T:System.ArgumentException">criteria包含的信息无效。
    ///     </exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<Row> GetRowsByOffset(OffsetRowQueryCriteria criteria, string transactionId);

    /// <summary>
    /// 开始异步操作，获取表（Table）或视图（View）的指定偏移量开始的多行数据。
    /// 
    /// </summary>
    /// <param name="criteria">表示查询条件的<see cref="T:Aliyun.OpenServices.OpenTableService.OffsetRowQueryCriteria"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">criteria包含的信息无效。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginGetRowsByOffset(OffsetRowQueryCriteria criteria, AsyncCallback callback, object state);

    IAsyncResult BeginGetRowsByOffset(OffsetRowQueryCriteria criteria, string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束异步操作，获取表（Table）或视图（View）的指定偏移量开始的多行数据。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    IEnumerable<Row> EndGetRowsByOffset(IAsyncResult asyncResult);

    /// <summary>
    /// 插入一行或修改指定行中的数据。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
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
    void PutData(string tableName, RowPutChange rowChange);

    /// <summary>
    /// 插入一行或修改指定行中的数据。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    /// <example>
    /// 下面的示例代码演示如何通过<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的方法在OTS中创建一个表（Table）。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using Aliyun.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class CreateTableSample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    /// 
    ///         string tableName = "contact_table";
    ///         string viewName = "view1";
    /// 
    ///         public void CreateTable()
    ///         {
    ///             // 创建表结构信息。
    ///             var tableMeta = new TableMeta(tableName);
    ///             // 指定表的主键。
    ///             tableMeta.PrimaryKeys.Add("uid", PrimaryKeyType.Integer);
    ///             tableMeta.PrimaryKeys.Add("flag", PrimaryKeyType.Boolean);
    ///             tableMeta.PrimaryKeys.Add("name", PrimaryKeyType.String);
    ///             tableMeta.PagingKeyLength = 2;
    /// 
    ///             var viewMeta = new ViewMeta(viewName);
    ///             viewMeta.PrimaryKeys.Add("uid", PrimaryKeyType.Integer);
    ///             viewMeta.PrimaryKeys.Add("flag", PrimaryKeyType.Boolean);
    ///             tableMeta.PrimaryKeys.Add("name", PrimaryKeyType.String);
    ///             viewMeta.PrimaryKeys.Add("groupid", PrimaryKeyType.Integer);
    /// 
    ///             tableMeta.Views.Add(viewMeta);
    /// 
    ///             // 在OTS中创建一个表。
    ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
    /// 
    ///             try
    ///             {
    ///                 otsClient.CreateTable(tableMeta);
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("创建表失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常。
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
    void PutData(string tableName, RowPutChange rowChange, string transactionId);

    /// <summary>
    /// 开始插入一行或修改指定行中的数据的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginPutData(string tableName, RowPutChange rowChange, AsyncCallback callback, object state);

    /// <summary>
    /// 开始插入一行或修改指定行中的数据的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowPutChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginPutData(string tableName, RowPutChange rowChange, string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束插入一行或修改指定行中的数据的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndPutData(IAsyncResult asyncResult);

    /// <summary>
    /// 删除指定行或行中的数据。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void DeleteData(string tableName, RowDeleteChange rowChange);

    /// <summary>
    /// 删除指定行或行中的数据。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void DeleteData(string tableName, RowDeleteChange rowChange, string transactionId);

    /// <summary>
    /// 开始删除指定行或行中的数据的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginDeleteData(string tableName, RowDeleteChange rowChange, AsyncCallback callback, object state);

    /// <summary>
    /// 开始删除指定行或行中的数据的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChange">包含数据的<see cref="T:Aliyun.OpenServices.OpenTableService.RowDeleteChange"/>对象。
    ///     </param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChange包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">row为空引用。</exception><param name="transactionId">事务ID，不为空时表示该操作在该事务中进行。</param><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    IAsyncResult BeginDeleteData(string tableName, RowDeleteChange rowChange, string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束删除指定行或行中的数据的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndDeleteData(IAsyncResult asyncResult);

    /// <summary>
    /// 把PutData和/或DeleteData的多次调用组合成一个调用。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 要求这个组合调用必须在事务中进行，且所有数据更改的操作必须操作同一个数据分片键（Partition Key）的数据，
    ///       并且这个数据分片键要和开始事务（Transaction）的数据分片键相同。
    /// 
    /// </remarks>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChanges"><see cref="T:Aliyun.OpenServices.OpenTableService.RowChange"/>对象的枚举器。
    ///     </param><param name="transactionId">事务（Transaction）ID。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChanges包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">rowChanges为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void BatchModifyData(string tableName, IEnumerable<RowChange> rowChanges, string transactionId);

    /// <summary>
    /// 开始把PutData和/或DeleteData的多次调用组合成一个调用的异步操作。
    /// 
    /// </summary>
    /// <param name="tableName">表（Table）名，不能为视图（View）名。</param><param name="rowChanges"><see cref="T:Aliyun.OpenServices.OpenTableService.RowChange"/>对象的枚举器。
    ///     </param><param name="transactionId">事务（Transaction）ID。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// tableName为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// talbeName违反OTS名称的命名规则。
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// rowChanges包含的信息无效。
    /// </para>
    /// </exception><exception cref="T:System.ArgumentNullException">rowChanges为空引用。</exception><param name="callback"><see cref="T:System.AsyncCallback"/>委托。
    ///     </param><param name="state">此请求的状态对象。</param>
    /// <returns>
    /// 引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    /// 
    /// </returns>
    /// 
    /// <example>
    /// 下面的示例代码演示如何开始一个事务（Transaction），并异步地批量提交数据修改。该示例中使用<see cref="M:Aliyun.OpenServices.OpenTableService.OtsClient.CreateTable(Aliyun.OpenServices.OpenTableService.TableMeta)"/>方法示例代码所创建的表。
    /// 
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Linq;
    /// using System.Net;
    /// using Aliyun.OpenServices.OpenTableService;
    /// 
    /// namespace Aliyun.OpenServices.Samples.OpenTableService
    /// {
    ///     class AsyncBatchModifyDataSample
    ///     {
    ///         string endpoint = "http://ots.aliyuncs.com";
    ///         string accessId = "<your access id>";
    ///         string accessKey = "<your access key>";
    ///         OtsClient otsClient;
    /// 
    ///         public AsyncBatchModifyDataSample()
    ///         {
    ///             otsClient = new OtsClient(endpoint, accessId, accessKey);
    ///         }
    /// 
    ///         public void BatchInsertData(string tableName)
    ///         {
    ///             int uid = 1;
    ///             // 将5条RowPutChange放到列表中
    ///             var rowChanges = new List<RowChange>();
    /// 
    ///             for (int i = 0; i < 5; i++)
    ///             {
    ///                 var rowChange = new RowPutChange();
    ///                 rowChange.PrimaryKeys["uid"] = uid;
    ///                 rowChange.PrimaryKeys["flag"] = true;
    ///                 rowChange.PrimaryKeys["name"] = "contact " + i.ToString();
    /// 
    ///                 rowChange.AttributeColumns["groupid"] = 1;
    ///                 rowChange.AttributeColumns["address"] = "中国某地";
    /// 
    ///                 rowChanges.Add(rowChange);
    ///             }
    /// 
    ///             // 开始一个事务
    ///             try
    ///             {
    ///                 var transactionId = otsClient.StartTransaction(tableName, uid);
    /// 
    ///                 // 开始批量修改数据的异步操作
    ///                 otsClient.BeginBatchModifyData(tableName, rowChanges, transactionId, BatchModifyDataCallback, transactionId);
    /// 
    ///                 // 这里进行其他操作
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    /// 
    ///         private void BatchModifyDataCallback(IAsyncResult asyncResult)
    ///         {
    ///             try
    ///             {
    ///                 var transactionId = asyncResult.AsyncState as string;
    /// 
    ///                 // 操作结束时必须调用EndBatchModifyData方法，否则可能会产生资源泄露
    ///                 otsClient.EndBatchModifyData(asyncResult);
    /// 
    ///                 // 需要调用ComitTransaction方法提交数据修改
    ///                 otsClient.CommitTransaction(transactionId);
    ///             }
    ///             catch (OtsException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。OTS异常消息：" + ex.Message);
    ///                 // RequestId和HostId可以在有问题时用于联系客服诊断异常
    ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
    ///             }
    ///             catch (WebException ex)
    ///             {
    ///                 Console.WriteLine("操作失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
    ///             }
    ///         }
    ///     }
    /// }
    ///         ]]>
    /// </code>
    /// 
    /// </example>
    IAsyncResult BeginBatchModifyData(string tableName, IEnumerable<RowChange> rowChanges, string transactionId, AsyncCallback callback, object state);

    /// <summary>
    /// 结束把PutData和/或DeleteData的多次调用组合成一个调用的异步操作。
    /// 
    /// </summary>
    /// <param name="asyncResult">引用该异步请求的<see cref="T:System.IAsyncResult"/>对象。
    ///     </param><exception cref="T:System.ArgumentNullException">asyncResult为空引用。</exception><exception cref="T:Aliyun.OpenServices.OpenTableService.OtsException">
    /// <para>
    /// OTS访问返回错误消息。
    /// </para>
    /// </exception><exception cref="T:System.Net.WebException">
    /// <para>
    /// 由于网络原因请求失败，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// 访问超时。
    /// </para>
    /// </exception><exception cref="T:System.InvalidOperationException">
    /// <para>
    /// 返回结果解析错误。
    /// </para>
    /// </exception>
    void EndBatchModifyData(IAsyncResult asyncResult);
  }
}
