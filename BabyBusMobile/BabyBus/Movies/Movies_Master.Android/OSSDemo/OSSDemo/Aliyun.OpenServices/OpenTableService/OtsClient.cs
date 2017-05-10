// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.OtsClient
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using Aliyun.OpenServices.OpenTableService.Commands;
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.Utilities;
using Aliyun.OpenServices.Properties;
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
  public class OtsClient : IOts
  {
    private readonly Uri _endpoint;
    private readonly ServiceCredentials _credentials;
    private readonly ClientConfiguration _configuration;

    /// <summary>
    /// 获取表示OTS终结点的值。
    /// 
    /// </summary>
    public Uri Endpoint
    {
      get
      {
        return this._endpoint;
      }
    }

    /// <summary>
    /// 获取访问OTS的Access ID的值。
    /// 
    /// </summary>
    public string AccessId
    {
      get
      {
        return this._credentials.AccessId;
      }
    }

    /// <summary>
    /// 获取客户端配置的<see cref="T:Aliyun.OpenServices.ClientConfiguration"/>实例。
    /// 
    /// </summary>
    [Obsolete("请使用OtsClient的构造函数传入ClientConfiguration的对象，而不要使用该属性进行配置。")]
    public ClientConfiguration Configuration
    {
      get
      {
        return this._configuration;
      }
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的新实例。
    /// 
    /// </summary>
    /// <param name="accessId">访问OTS使用的Access ID。</param><param name="accessKey">访问OTS使用的Access key。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// endpoint为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// endpoint指定的协议不是http://或https://，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// accessID为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// accessKey为空引用或值为空字符串。
    /// </para>
    /// </exception>
    public OtsClient(string accessId, string accessKey)
      : this("http://ots.aliyuncs.com", accessId, accessKey)
    {
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的新实例。
    /// 
    /// </summary>
    /// <param name="accessId">访问OTS使用的Access ID。</param><param name="accessKey">访问OTS使用的Access key。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// endpoint为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// endpoint指定的协议不是http://或https://，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// accessID为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// accessKey为空引用或值为空字符串。
    /// </para>
    /// </exception><param name="endpoint">访问OTS的终结点值。</param>
    public OtsClient(string endpoint, string accessId, string accessKey)
      : this(endpoint, accessId, accessKey, (ClientConfiguration) null)
    {
    }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.OtsClient"/>的新实例。
    /// 
    /// </summary>
    /// <param name="accessId">访问OTS使用的Access ID。</param><param name="accessKey">访问OTS使用的Access key。</param><exception cref="T:System.ArgumentException">
    /// <para>
    /// endpoint为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// endpoint指定的协议不是http://或https://，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// accessID为空引用或值为空字符串，
    /// </para>
    /// 
    /// <para>
    /// - 或 -
    /// </para>
    /// 
    /// <para>
    /// accessKey为空引用或值为空字符串。
    /// </para>
    /// </exception><param name="endpoint">访问OTS的终结点值。</param><param name="configuration"><see cref="T:Aliyun.OpenServices.ClientConfiguration"/>.</param>
    public OtsClient(string endpoint, string accessId, string accessKey, ClientConfiguration configuration)
    {
      if (string.IsNullOrEmpty(accessId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
      if (string.IsNullOrEmpty(accessKey))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");
      if (string.IsNullOrEmpty(endpoint))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "endpoint");
      if (!endpoint.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException(OtsExceptions.EndpointNotSupportedProtocal, "endpoint");
      this._endpoint = new Uri(endpoint);
      this._credentials = new ServiceCredentials(accessId, accessKey);
      this._configuration = configuration != null ? (ClientConfiguration) configuration.Clone() : new ClientConfiguration();
    }

    /// <inheritdoc/>
    public void CreateTableGroup(string tableGroupName, PartitionKeyType partitionKeyType)
    {
      CreateTableGroupCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("CreateTableGroup"), tableGroupName, partitionKeyType).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginCreateTableGroup(string tableGroupName, PartitionKeyType partitionKeyType, AsyncCallback callback, object state)
    {
      return CreateTableGroupCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("CreateTableGroup"), tableGroupName, partitionKeyType).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndCreateTableGroup(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public IEnumerable<string> ListTableGroups()
    {
      return (IEnumerable<string>) ListTableGroupCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("ListTableGroup")).Execute().TableGroupNames;
    }

    /// <inheritdoc/>
    public IAsyncResult BeginListTableGroups(AsyncCallback callback, object state)
    {
      return ListTableGroupCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("ListTableGroup")).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public IEnumerable<string> EndListTableGroups(IAsyncResult asyncResult)
    {
      return (IEnumerable<string>) OtsCommand<ListTableGroupResult>.EndExecute(this.GetServiceClient(), asyncResult).TableGroupNames;
    }

    /// <inheritdoc/>
    public void DeleteTableGroup(string tableGroupName)
    {
      DeleteTableGroupCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("DeleteTableGroup"), tableGroupName).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginDeleteTableGroup(string tableGroupName, AsyncCallback callback, object state)
    {
      return DeleteTableGroupCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("DeleteTableGroup"), tableGroupName).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndDeleteTableGroup(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public void CreateTable(TableMeta tableMeta)
    {
      CreateTableCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("CreateTable"), tableMeta).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginCreateTable(TableMeta tableMeta, AsyncCallback callback, object state)
    {
      return CreateTableCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("CreateTable"), tableMeta).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndCreateTable(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public TableMeta GetTableMeta(string tableName)
    {
      return GetTableMetaCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetTableMeta"), tableName).Execute().TableMeta.ToTableMeta();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetTableMeta(string tableName, AsyncCallback callback, object state)
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      else
        return GetTableMetaCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetTableMeta"), tableName).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public TableMeta EndGetTableMeta(IAsyncResult asyncResult)
    {
      return OtsCommand<GetTableMetaResult>.EndExecute(this.GetServiceClient(), asyncResult).TableMeta.ToTableMeta();
    }

    /// <inheritdoc/>
    public IEnumerable<string> ListTables()
    {
      return (IEnumerable<string>) ListTableCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("ListTable")).Execute().TableNames;
    }

    /// <inheritdoc/>
    public IAsyncResult BeginListTables(AsyncCallback callback, object state)
    {
      return ListTableCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("ListTable")).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public IEnumerable<string> EndListTables(IAsyncResult asyncResult)
    {
      return (IEnumerable<string>) OtsCommand<ListTableResult>.EndExecute(this.GetServiceClient(), asyncResult).TableNames;
    }

    /// <inheritdoc/>
    public void DeleteTable(string tableName)
    {
      DeleteTableCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("DeleteTable"), tableName).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginDeleteTable(string tableName, AsyncCallback callback, object state)
    {
      return DeleteTableCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("DeleteTable"), tableName).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndDeleteTable(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public string StartTransaction(string entityName, PartitionKeyValue partitionKeyValue)
    {
      return StartTransactionCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("StartTransaction"), entityName, partitionKeyValue).Execute().TransactionId;
    }

    /// <inheritdoc/>
    public IAsyncResult BeginStartTransaction(string entityName, PartitionKeyValue partitionKeyValue, AsyncCallback callback, object state)
    {
      return StartTransactionCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("StartTransaction"), entityName, partitionKeyValue).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public string EndStartTransaction(IAsyncResult asyncResult)
    {
      return OtsCommand<StartTransactionResult>.EndExecute(this.GetServiceClient(), asyncResult).TransactionId;
    }

    /// <inheritdoc/>
    public void CommitTransaction(string transactionId)
    {
      CommitTransactionCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("CommitTransaction"), transactionId).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginCommitTransaction(string transactionId, AsyncCallback callback, object state)
    {
      return CommitTransactionCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("CommitTransaction"), transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndCommitTransaction(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public void AbortTransaction(string transactionId)
    {
      AbortTransactionCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("AbortTransaction"), transactionId).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginAbortTransaction(string transactionId, AsyncCallback callback, object state)
    {
      return AbortTransactionCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("AbortTransaction"), transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndAbortTransaction(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public Row GetRow(SingleRowQueryCriteria criteria)
    {
      return this.GetRow(criteria, (string) null);
    }

    /// <inheritdoc/>
    public Row GetRow(SingleRowQueryCriteria criteria, string transactionId)
    {
      return GetRowCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetRow"), criteria, transactionId).Execute().GetSingleRow();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria, AsyncCallback callback, object state)
    {
      return this.BeginGetRow(criteria, (string) null, callback, state);
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetRow(SingleRowQueryCriteria criteria, string transactionId, AsyncCallback callback, object state)
    {
      return GetRowCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetRow"), criteria, transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public Row EndGetRow(IAsyncResult asyncResult)
    {
      return OtsCommand<GetRowResult>.EndExecute(this.GetServiceClient(), asyncResult).GetSingleRow();
    }

    /// <inheritdoc/>
    public IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria)
    {
      return this.GetRowsByRange(criteria, (string) null);
    }

    /// <inheritdoc/>
    public IEnumerable<Row> GetRowsByRange(RangeRowQueryCriteria criteria, string transactionId)
    {
      return GetRowsByRangeCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetRowsByRange"), criteria, transactionId).Execute().GetMultipleRows();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria, AsyncCallback callback, object state)
    {
      return this.BeginGetRowsByRange(criteria, (string) null, callback, state);
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetRowsByRange(RangeRowQueryCriteria criteria, string transactionId, AsyncCallback callback, object state)
    {
      return GetRowsByRangeCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetRowsByRange"), criteria, transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public IEnumerable<Row> EndGetRowsByRange(IAsyncResult asyncResult)
    {
      return OtsCommand<GetRowsByRangeResult>.EndExecute(this.GetServiceClient(), asyncResult).GetMultipleRows();
    }

    /// <inheritdoc/>
    public IEnumerable<Row> GetRowsByOffset(OffsetRowQueryCriteria criteria)
    {
      return this.GetRowsByOffset(criteria, (string) null);
    }

    /// <inheritdoc/>
    public IEnumerable<Row> GetRowsByOffset(OffsetRowQueryCriteria criteria, string transactionId)
    {
      return GetRowsByOffsetCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetRowsByOffset"), criteria, transactionId).Execute().GetMultipleRows();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetRowsByOffset(OffsetRowQueryCriteria criteria, AsyncCallback callback, object state)
    {
      return this.BeginGetRowsByOffset(criteria, (string) null, callback, state);
    }

    /// <inheritdoc/>
    public IAsyncResult BeginGetRowsByOffset(OffsetRowQueryCriteria criteria, string transactionId, AsyncCallback callback, object state)
    {
      return GetRowsByOffsetCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("GetRowsByOffset"), criteria, transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public IEnumerable<Row> EndGetRowsByOffset(IAsyncResult asyncResult)
    {
      return OtsCommand<GetRowsByOffsetResult>.EndExecute(this.GetServiceClient(), asyncResult).GetMultipleRows();
    }

    /// <inheritdoc/>
    public void PutData(string tableName, RowPutChange rowChange)
    {
      this.PutData(tableName, rowChange, (string) null);
    }

    /// <inheritdoc/>
    public void PutData(string tableName, RowPutChange rowChange, string transactionId)
    {
      PutDataCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("PutData"), tableName, rowChange, transactionId).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginPutData(string tableName, RowPutChange rowChange, AsyncCallback callback, object state)
    {
      return this.BeginPutData(tableName, rowChange, (string) null, callback, state);
    }

    /// <inheritdoc/>
    public IAsyncResult BeginPutData(string tableName, RowPutChange rowChange, string transactionId, AsyncCallback callback, object state)
    {
      return PutDataCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("PutData"), tableName, rowChange, transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndPutData(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public void DeleteData(string tableName, RowDeleteChange rowChange)
    {
      this.DeleteData(tableName, rowChange, (string) null);
    }

    /// <inheritdoc/>
    public void DeleteData(string tableName, RowDeleteChange rowChange, string transactionId)
    {
      DeleteDataCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("DeleteData"), tableName, rowChange, transactionId).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginDeleteData(string tableName, RowDeleteChange rowChange, AsyncCallback callback, object state)
    {
      return this.BeginDeleteData(tableName, rowChange, (string) null, callback, state);
    }

    /// <inheritdoc/>
    public IAsyncResult BeginDeleteData(string tableName, RowDeleteChange rowChange, string transactionId, AsyncCallback callback, object state)
    {
      return DeleteDataCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("DeleteData"), tableName, rowChange, transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndDeleteData(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    /// <inheritdoc/>
    public void BatchModifyData(string tableName, IEnumerable<RowChange> rowChanges, string transactionId)
    {
      BatchModifyDataCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("BatchModifyData"), tableName, rowChanges, transactionId).Execute();
    }

    /// <inheritdoc/>
    public IAsyncResult BeginBatchModifyData(string tableName, IEnumerable<RowChange> rowChanges, string transactionId, AsyncCallback callback, object state)
    {
      return BatchModifyDataCommand.Create(this.GetServiceClient(), this._endpoint, this.CreateContext("BatchModifyData"), tableName, rowChanges, transactionId).BeginExecute(callback, state);
    }

    /// <inheritdoc/>
    public void EndBatchModifyData(IAsyncResult asyncResult)
    {
      OtsCommand.EndExecute(this.GetServiceClient(), asyncResult);
    }

    private IServiceClient GetServiceClient()
    {
      return ServiceClientFactory.CreateServiceClient(this._configuration);
    }

    private ExecutionContext CreateContext(string action)
    {
      ExecutionContext executionContext = new ExecutionContext();
      executionContext.Signer = (IRequestSigner) new OtsRequestSigner();
      executionContext.Credentials = this._credentials;
      executionContext.ResponseHandlers.Add((IResponseHandler) new OtsExceptionHandler());
      executionContext.ResponseHandlers.Add((IResponseHandler) new ValidationResponseHandler(this._credentials, action));
      return executionContext;
    }
  }
}
