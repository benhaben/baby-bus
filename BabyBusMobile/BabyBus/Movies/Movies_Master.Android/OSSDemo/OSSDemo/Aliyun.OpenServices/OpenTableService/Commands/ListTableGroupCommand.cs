// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Commands.ListTableGroupCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Model;
using System;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
  /// <summary>
  /// Description of ListTableGroupsCommand.
  /// 
  /// </summary>
  internal class ListTableGroupCommand : OtsCommand<ListTableGroupResult>
  {
    public const string ActionName = "ListTableGroup";

    protected override string ResourcePath
    {
      get
      {
        return "ListTableGroup";
      }
    }

    protected override HttpMethod Method
    {
      get
      {
        return HttpMethod.Get;
      }
    }

    protected ListTableGroupCommand(IServiceClient client, Uri endpoint, ExecutionContext context)
      : base(client, endpoint, context)
    {
    }

    public static ListTableGroupCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context)
    {
      return new ListTableGroupCommand(client, endpoint, context);
    }
  }
}
