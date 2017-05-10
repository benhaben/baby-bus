// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.DeleteDataCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of DeleteDataCommand.
  /// 
  /// </summary>
  internal class DeleteDataCommand : OtsCommand
  {
    public const string ActionName = "DeleteData";
    private string _tableName;
    private string _transactionId;
    private RowDeleteChange _rowChange;

    protected override string ResourcePath
    {
      get
      {
        return "DeleteData";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected DeleteDataCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string tableName, RowDeleteChange rowChange, string transactionId)
      : base(client, endpoint, context)
    {
      Debug.Assert(!string.IsNullOrEmpty(tableName) && OtsUtility.IsEntityNameValid(tableName) && rowChange != null);
      this._tableName = tableName;
      this._rowChange = rowChange;
      this._transactionId = transactionId;
    }

    public static DeleteDataCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string tableName, RowDeleteChange rowChange, string transactionId)
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      DeleteDataCommand.ValidateRowChange(rowChange);
      return new DeleteDataCommand(client, endpoint, context, tableName, rowChange, transactionId);
    }

    private static void ValidateRowChange(RowDeleteChange rowChange)
    {
      if (rowChange == null)
        throw new ArgumentNullException("rowChange");
      if (rowChange.PrimaryKeys.Count == 0)
        throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "rowChange");
      OtsUtility.AssertColumnNames((IEnumerable<string>) rowChange.PrimaryKeys.Keys);
      OtsUtility.AssertColumnNames((IEnumerable<string>) rowChange.ColumnNames);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableName", this._tableName);
      int num1 = 1;
      foreach (KeyValuePair<string, PrimaryKeyValue> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyValue>>) this._rowChange.PrimaryKeys)
      {
        string str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "PK.{0}.", new object[1]
        {
          (object) num1++
        });
        parameters.Add(str + "Name", keyValuePair.Key);
        parameters.Add(str + "Value", TableValueExtension.ToParameterString(keyValuePair.Value));
        parameters.Add(str + "Type", PrimaryKeyTypeHelper.GetString(keyValuePair.Value.ValueType));
      }
      int num2 = 1;
      foreach (string str in (IEnumerable<string>) this._rowChange.ColumnNames)
        parameters.Add(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Column.{0}.Name", new object[1]
        {
          (object) num2++
        }), str);
      if (string.IsNullOrEmpty(this._transactionId))
        return;
      parameters.Add("TransactionID", this._transactionId);
    }
  }
}
