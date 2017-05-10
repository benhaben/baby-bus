// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.AbortTransactionCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Properties;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of AbortTransactionCommand.
  /// 
  /// </summary>
  internal class AbortTransactionCommand : OtsCommand
  {
    public const string ActionName = "AbortTransaction";
    private string _transactionId;

    protected override string ResourcePath
    {
      get
      {
        return "AbortTransaction";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Get;
      }
    }

    protected AbortTransactionCommand(IServiceClient client, Uri endpoint, ExecutionContext context, string transactionId)
      : base(client, endpoint, context)
    {
      if (string.IsNullOrEmpty(transactionId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "transactionId");
      this._transactionId = transactionId;
    }

    public static AbortTransactionCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context, string transactionId)
    {
      return new AbortTransactionCommand(client, endpoint, context, transactionId);
    }

    protected override void AddRequestParameters(IDictionary<string, string> parameters)
    {
      parameters.Add("TransactionID", this._transactionId);
    }
  }
}
