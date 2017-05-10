// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.GetRowCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of GetRowCommand.
  /// 
  /// </summary>
  internal class GetRowCommand : OtsCommand<GetRowResult>
  {
    public const string ActionName = "GetRow";
    private SingleRowQueryCriteria _criteria;
    private string _transactionId;

    protected override string ResourcePath
    {
      get
      {
        return "GetRow";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected GetRowCommand(IServiceClient client, Uri endpoint, ExecutionContext context, SingleRowQueryCriteria criteria, string transactionId)
      : base(client, endpoint, context)
    {
      Debug.Assert(criteria != null);
      this._criteria = criteria;
      this._transactionId = transactionId;
    }

    public static GetRowCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, SingleRowQueryCriteria criteria, string transactionId)
    {
      GetRowCommand.ValidateParameter(criteria);
      return new GetRowCommand(client, endpoint, context, criteria, transactionId);
    }

    private static void ValidateParameter(SingleRowQueryCriteria criteria)
    {
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      if (criteria.PrimaryKeys.Count == 0)
        throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "criteria");
      OtsUtility.AssertColumnNames((IEnumerable<string>) criteria.PrimaryKeys.Keys);
      OtsUtility.AssertColumnNames((IEnumerable<string>) criteria.ColumnNames);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableName", OtsUtility.GetFullQueryTableName((RowQueryCriteria) this._criteria));
      int num1 = 1;
      foreach (KeyValuePair<string, PrimaryKeyValue> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyValue>>) this._criteria.PrimaryKeys)
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
      foreach (string str in (IEnumerable<string>) this._criteria.ColumnNames)
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
