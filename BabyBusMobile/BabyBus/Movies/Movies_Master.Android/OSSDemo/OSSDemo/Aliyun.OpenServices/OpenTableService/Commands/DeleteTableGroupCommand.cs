// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.DeleteTableGroupCommand
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
  /// Description of DeleteTableGroupCommand.
  /// 
  /// </summary>
  internal class DeleteTableGroupCommand : OtsCommand
  {
    public const string ActionName = "DeleteTableGroup";
    private string _tableGroupName;

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
        return "DeleteTableGroup";
      }
    }

    protected DeleteTableGroupCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string tableGroupName)
      : base(client, endpoint, context)
    {
      Debug.Assert(!string.IsNullOrEmpty(tableGroupName) && OtsUtility.IsEntityNameValid(tableGroupName));
      this._tableGroupName = tableGroupName;
    }

    public static DeleteTableGroupCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string tableGroupName)
    {
      if (string.IsNullOrEmpty(tableGroupName) || !OtsUtility.IsEntityNameValid(tableGroupName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableGroupName");
      else
        return new DeleteTableGroupCommand(client, endpoint, context, tableGroupName);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableGroupName", this._tableGroupName);
    }
  }
}
