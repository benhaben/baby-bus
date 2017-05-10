// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.GetRowsByRangeCommand
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
  /// Description of GetRowsByRangeCommand.
  /// 
  /// </summary>
  internal class GetRowsByRangeCommand : OtsCommand<GetRowsByRangeResult>
  {
    public const string ActionName = "GetRowsByRange";
    private RangeRowQueryCriteria _criteria;
    private string _transactionId;

    protected override string ResourcePath
    {
      get
      {
        return "GetRowsByRange";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected GetRowsByRangeCommand(IServiceClient client, Uri endpoint, ExecutionContext context, RangeRowQueryCriteria criteria, string transactionId)
      : base(client, endpoint, context)
    {
      Debug.Assert(criteria != null);
      this._criteria = criteria;
      this._transactionId = transactionId;
    }

    public static GetRowsByRangeCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, RangeRowQueryCriteria criteria, string transactionId)
    {
      GetRowsByRangeCommand.ValidateParameter(criteria);
      return new GetRowsByRangeCommand(client, endpoint, context, criteria, transactionId);
    }

    private static void ValidateParameter(RangeRowQueryCriteria criteria)
    {
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      OtsUtility.AssertColumnNames((IEnumerable<string>) criteria.PrimaryKeys.Keys);
      OtsUtility.AssertColumnNames((IEnumerable<string>) criteria.ColumnNames);
      if (criteria.Range == null)
        throw new ArgumentException(OtsExceptions.RangeNotSet, "criteria");
      PrimaryKeyValue rangeBegin = criteria.Range.RangeBegin;
      PrimaryKeyValue rangeEnd = criteria.Range.RangeEnd;
      if (!criteria.IsReverse && (rangeBegin == PrimaryKeyRange.InfMax || rangeEnd == PrimaryKeyRange.InfMin) || criteria.IsReverse && (rangeBegin == PrimaryKeyRange.InfMin || rangeEnd == PrimaryKeyRange.InfMax))
        throw new ArgumentException(OtsExceptions.PKInfNotAllowed);
      if (!criteria.IsReverse && rangeBegin.CompareTo(rangeEnd) > 0 || criteria.IsReverse && rangeBegin.CompareTo(rangeEnd) < 0)
        throw new ArgumentException(OtsExceptions.RangeBeginGreaterThanRangeEnd);
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
      string str1 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "PK.{0}.", new object[1]
      {
        (object) num1
      });
      parameters.Add(str1 + "Name", this._criteria.Range.PrimaryKeyName);
      parameters.Add(str1 + "RangeBegin", TableValueExtension.ToParameterString(this._criteria.Range.RangeBegin));
      parameters.Add(str1 + "RangeEnd", TableValueExtension.ToParameterString(this._criteria.Range.RangeEnd));
      parameters.Add(str1 + "RangeType", PrimaryKeyTypeHelper.GetString(this._criteria.Range.RangeBegin.ValueType));
      int num2 = 1;
      foreach (string str2 in (IEnumerable<string>) this._criteria.ColumnNames)
        parameters.Add(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Column.{0}.Name", new object[1]
        {
          (object) num2++
        }), str2);
      if (this._criteria.Top >= 0)
        parameters.Add("Top", this._criteria.Top.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (this._criteria.IsReverse)
        parameters.Add("IsReverse", this._criteria.IsReverse.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToUpperInvariant());
      if (string.IsNullOrEmpty(this._transactionId))
        return;
      parameters.Add("TransactionID", this._transactionId);
    }
  }
}
