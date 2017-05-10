// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.StartTransactionCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of StartTransactionCommand.
  /// 
  /// </summary>
  internal class StartTransactionCommand : OtsCommand<StartTransactionResult>
  {
    public const string ActionName = "StartTransaction";
    private string _entityName;
    private PartitionKeyValue _partitionKeyValue;

    protected override string ResourcePath
    {
      get
      {
        return "StartTransaction";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Get;
      }
    }

    protected StartTransactionCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string entityName, PartitionKeyValue partitionKeyValue)
      : base(client, endpoint, context)
    {
      this._entityName = entityName;
      this._partitionKeyValue = partitionKeyValue;
    }

    public static StartTransactionCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string entityName, PartitionKeyValue partitionKeyValue)
    {
      if (string.IsNullOrEmpty(entityName) || !OtsUtility.IsEntityNameValid(entityName))
        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "entityName");
      else
        return new StartTransactionCommand(client, endpoint, context, entityName, partitionKeyValue);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("EntityName", this._entityName);
      parameters.Add("PartitionKeyValue", TableValueExtension.ToParameterString(this._partitionKeyValue));
      parameters.Add("PartitionKeyType", PartitionKeyTypeHelper.GetString(this._partitionKeyValue.ValueType));
    }
  }
}
