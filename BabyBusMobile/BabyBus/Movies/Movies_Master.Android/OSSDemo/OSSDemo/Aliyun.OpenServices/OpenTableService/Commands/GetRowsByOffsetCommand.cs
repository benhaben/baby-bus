// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.GetRowsByOffsetCommand
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
  /// Description of GetRowsByOffsetCommand.
  /// 
  /// </summary>
  internal class GetRowsByOffsetCommand : OtsCommand<GetRowsByOffsetResult>
  {
    public const string ActionName = "GetRowsByOffset";
    private OffsetRowQueryCriteria _criteria;
    private string _transactionId;

    protected override string ResourcePath
    {
      get
      {
        return "GetRowsByOffset";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected GetRowsByOffsetCommand(IServiceClient client, Uri endpoint, ExecutionContext context, OffsetRowQueryCriteria criteria, string transactionId)
      : base(client, endpoint, context)
    {
      Debug.Assert(criteria != null);
      this._criteria = criteria;
      this._transactionId = transactionId;
    }

    public static GetRowsByOffsetCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, OffsetRowQueryCriteria criteria, string transactionId)
    {
      GetRowsByOffsetCommand.ValidateParameter(criteria);
      return new GetRowsByOffsetCommand(client, endpoint, context, criteria, transactionId);
    }

    private static void ValidateParameter(OffsetRowQueryCriteria criteria)
    {
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      if (criteria.PagingKeys.Count == 0)
        throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "criteria");
      if (!criteria.isTopSet)
        throw new ArgumentException(OtsExceptions.QueryTopValueNotSet, "criteria");
      OtsUtility.AssertColumnNames((IEnumerable<string>) criteria.PagingKeys.Keys);
      OtsUtility.AssertColumnNames((IEnumerable<string>) criteria.ColumnNames);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableName", OtsUtility.GetFullQueryTableName((RowQueryCriteria) this._criteria));
      int num1 = 1;
      foreach (KeyValuePair<string, PrimaryKeyValue> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyValue>>) this._criteria.PagingKeys)
      {
        string str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Paging.{0}.", new object[1]
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
      IDictionary<string, string> dictionary1 = parameters;
      string key1 = "Offset";
      int num3 = this._criteria.Offset;
      string str1 = num3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      dictionary1.Add(key1, str1);
      IDictionary<string, string> dictionary2 = parameters;
      string key2 = "Top";
      num3 = this._criteria.Top;
      string str2 = num3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      dictionary2.Add(key2, str2);
      if (this._criteria.IsReverse)
        parameters.Add("IsReverse", this._criteria.IsReverse.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToUpperInvariant());
      if (string.IsNullOrEmpty(this._transactionId))
        return;
      parameters.Add("TransactionID", this._transactionId);
    }
  }
}
