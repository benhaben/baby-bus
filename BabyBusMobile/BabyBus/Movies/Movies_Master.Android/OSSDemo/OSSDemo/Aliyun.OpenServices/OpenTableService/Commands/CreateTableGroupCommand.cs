// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.CreateTableGroupCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of CreateTableGroupCommand.
  /// 
  /// </summary>
  internal class CreateTableGroupCommand : OtsCommand
  {
    public const string ActionName = "CreateTableGroup";
    private string _tableGroupName;
    private PartitionKeyType _partitionKeyType;

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
        return "CreateTableGroup";
      }
    }

    protected CreateTableGroupCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string tableGroupName, PartitionKeyType partitionKeyType)
      : base(client, endpoint, context)
    {
      if (string.IsNullOrEmpty(tableGroupName) || !OtsUtility.IsEntityNameValid(tableGroupName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableGroupName");
      this._tableGroupName = tableGroupName;
      this._partitionKeyType = partitionKeyType;
    }

    public static CreateTableGroupCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string tableGroupName, PartitionKeyType partitionKeyType)
    {
      return new CreateTableGroupCommand(client, endpoint, context, tableGroupName, partitionKeyType);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TableGroupName", this._tableGroupName);
      parameters.Add("PartitionKeyType", PartitionKeyTypeHelper.GetString(this._partitionKeyType));
    }
  }
}
