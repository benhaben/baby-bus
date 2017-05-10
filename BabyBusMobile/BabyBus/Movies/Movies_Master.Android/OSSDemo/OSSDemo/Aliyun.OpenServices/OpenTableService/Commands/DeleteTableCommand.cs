// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.DeleteTableCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of DeleteTableCommand.
  /// 
  /// </summary>
  internal class DeleteTableCommand : OtsCommand
  {
    public const string ActionName = "DeleteTable";
    private string _tableName;

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Get;
      }
    }

    protected override string ResourcePath
    {
      get
      {
        return "DeleteTable";
      }
    }

    protected DeleteTableCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string tableName)
      : base(client, endpoint, context)
    {
      Debug.Assert(!string.IsNullOrEmpty(tableName) && OtsUtility.IsEntityNameValid(tableName));
      this._tableName = tableName;
    }

    public static DeleteTableCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string tableName)
    {
      if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");
      else
        return new DeleteTableCommand(client, endpoint, context, tableName);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableName", this._tableName);
    }
  }
}
