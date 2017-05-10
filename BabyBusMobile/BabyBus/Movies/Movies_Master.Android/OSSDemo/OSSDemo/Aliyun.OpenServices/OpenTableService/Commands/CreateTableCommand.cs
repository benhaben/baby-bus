// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.CreateTableCommand
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
  /// The command to create a table.
  /// 
  /// </summary>
  internal class CreateTableCommand : OtsCommand
  {
    public const string ActionName = "CreateTable";
    private TableMeta _tableMeta;

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Post;
      }
    }

    protected override string ResourcePath
    {
      get
      {
        return "CreateTable";
      }
    }

    protected CreateTableCommand(IServiceClient client, Uri endpoint, ExecutionContext context, TableMeta tableMeta)
      : base(client, endpoint, context)
    {
      Debug.Assert(tableMeta != null);
      this._tableMeta = tableMeta;
    }

    public static CreateTableCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, TableMeta tableMeta)
    {
      CreateTableCommand.ValidateTableMeta(tableMeta);
      return new CreateTableCommand(client, endpoint, context, tableMeta);
    }

    private static void ValidateTableMeta(TableMeta tableMeta)
    {
      if (tableMeta == null)
        throw new ArgumentNullException("tableMeta");
      if (string.IsNullOrEmpty(tableMeta.TableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableMeta");
      if (tableMeta.PrimaryKeys.Count == 0)
        throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "tableMeta");
      OtsUtility.AssertColumnNames((IEnumerable<string>) tableMeta.PrimaryKeys.Keys);
      if (tableMeta.PagingKeyLength < 0 || tableMeta.PagingKeyLength >= tableMeta.PrimaryKeys.Count)
        throw new ArgumentException(OtsExceptions.InvalidPagingKeyLength, "tableMeta");
      foreach (ViewMeta viewMeta in (IEnumerable<ViewMeta>) tableMeta.Views)
      {
        Debug.Assert(OtsUtility.IsEntityNameValid(viewMeta.ViewName), "ViewName should have been validated when it is added.");
        OtsUtility.AssertColumnNames((IEnumerable<string>) viewMeta.PrimaryKeys.Keys);
        OtsUtility.AssertColumnNames((IEnumerable<string>) viewMeta.AttributeColumns.Keys);
        if (viewMeta.PagingKeyLength < 0 || viewMeta.PagingKeyLength >= viewMeta.PrimaryKeys.Count)
          throw new ArgumentException(OtsExceptions.InvalidPagingKeyLength, "tableMeta");
      }
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableName", this._tableMeta.TableName);
      int num1 = 1;
      foreach (KeyValuePair<string, PrimaryKeyType> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyType>>) this._tableMeta.PrimaryKeys)
      {
        string str = "PK." + num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        parameters.Add(str + ".Name", keyValuePair.Key);
        parameters.Add(str + ".Type", PrimaryKeyTypeHelper.GetString(keyValuePair.Value));
        ++num1;
      }
      if (this._tableMeta.PagingKeyLength > 0)
        parameters.Add("PagingKeyLen", this._tableMeta.PagingKeyLength.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      int num2 = 1;
      foreach (ViewMeta viewMeta in (IEnumerable<ViewMeta>) this._tableMeta.Views)
      {
        string str1 = "View." + num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        parameters.Add(str1 + ".Name", viewMeta.ViewName);
        int num3 = 1;
        foreach (KeyValuePair<string, PrimaryKeyType> keyValuePair in (IEnumerable<KeyValuePair<string, PrimaryKeyType>>) viewMeta.PrimaryKeys)
        {
          string str2 = str1 + ".PK." + num3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          parameters.Add(str2 + ".Name", keyValuePair.Key);
          parameters.Add(str2 + ".Type", PrimaryKeyTypeHelper.GetString(keyValuePair.Value));
          ++num3;
        }
        int num4 = 1;
        foreach (KeyValuePair<string, ColumnType> keyValuePair in (IEnumerable<KeyValuePair<string, ColumnType>>) viewMeta.AttributeColumns)
        {
          string str2 = str1 + ".Column." + num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          parameters.Add(str2 + ".Name", keyValuePair.Key);
          parameters.Add(str2 + ".Type", ColumnTypeHelper.GetString(keyValuePair.Value));
          ++num4;
        }
        if (viewMeta.PagingKeyLength > 0)
          parameters.Add(str1 + ".PagingKeyLen", viewMeta.PagingKeyLength.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        ++num2;
      }
      if (string.IsNullOrEmpty(this._tableMeta.TableGroupName))
        return;
      parameters.Add("TableGroupName", this._tableMeta.TableGroupName);
    }
  }
}
