// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.BatchModifyDataCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of BatchModifyDataCommand.
  /// 
  /// </summary>
  internal class BatchModifyDataCommand : OtsCommand
  {
    public const string ActionName = "BatchModifyData";
    private string _tableName;
    private string _transactionId;
    private IEnumerable<RowChange> _rowChanges;

    protected override string ResourcePath
    {
      get
      {
        return "BatchModifyData";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected BatchModifyDataCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string tableName, IEnumerable<RowChange> rowChanges, string transactionId)
      : base(client, endpoint, context)
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      if (string.IsNullOrEmpty(transactionId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "transactionId");
      this._tableName = tableName;
      this._rowChanges = rowChanges;
      this._transactionId = transactionId;
    }

    public static BatchModifyDataCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string tableName, IEnumerable<RowChange> rowChanges, string transactionId)
    {
      BatchModifyDataCommand.ValidateRowChanges(rowChanges);
      return new BatchModifyDataCommand(client, endpoint, context, tableName, rowChanges, transactionId);
    }

    private static void ValidateRowChanges(IEnumerable<RowChange> rowChanges)
    {
      if (rowChanges == null)
        throw new ArgumentNullException("rowChanges");
      List<RowChange> list = Enumerable.ToList<RowChange>(rowChanges);
      if (list.Count == 0)
        throw new ArgumentException(OtsExceptions.NoRowForBatchModifyData, "rowChanges");
      foreach (RowChange rowChange in list)
      {
        if (rowChange.PrimaryKeys.Count == 0)
          throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "rowChanges");
      }
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableName", this._tableName);
      int num1 = 1;
      foreach (RowChange rowChange in this._rowChanges)
      {
        string str1 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Modify.{0}.", new object[1]
        {
          (object) num1++
        });
        parameters.Add(str1 + "Type", ((object) rowChange.ModifyType).ToString().ToUpperInvariant());
        int num2 = 1;
        foreach (KeyValuePair<string, PrimaryKeyValue> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyValue>>) rowChange.PrimaryKeys)
        {
          string str2 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}PK.{1}.", new object[2]
          {
            (object) str1,
            (object) num2++
          });
          parameters.Add(str2 + "Name", keyValuePair.Key);
          parameters.Add(str2 + "Value", TableValueExtension.ToParameterString(keyValuePair.Value));
          parameters.Add(str2 + "Type", PrimaryKeyTypeHelper.GetString(keyValuePair.Value.ValueType));
        }
        int num3 = 1;
        RowPutChange rowPutChange = rowChange as RowPutChange;
        if (rowPutChange != null)
        {
          foreach (KeyValuePair<string, ColumnValue> keyValuePair in (IEnumerable<KeyValuePair<string, ColumnValue>>) rowPutChange.AttributeColumns)
          {
            string str2 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}Column.{1}.", new object[2]
            {
              (object) str1,
              (object) num3++
            });
            parameters.Add(str2 + "Name", keyValuePair.Key);
            parameters.Add(str2 + "Value", TableValueExtension.ToParameterString(keyValuePair.Value));
            parameters.Add(str2 + "Type", ColumnTypeHelper.GetString(keyValuePair.Value.ValueType));
          }
          parameters.Add(str1 + "Checking", ((object) rowPutChange.CheckingMode).ToString().ToUpperInvariant());
        }
        else
        {
          foreach (string str2 in (IEnumerable<string>) (rowChange as RowDeleteChange).ColumnNames)
          {
            string key = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}Column.{1}.Name", new object[2]
            {
              (object) str1,
              (object) num3++
            });
            parameters.Add(key, str2);
          }
        }
      }
      parameters.Add("TransactionID", this._transactionId);
    }
  }
}
