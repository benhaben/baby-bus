// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.ListBucketsCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Transform;
using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.OpenStorageService.Transform;
using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Description of ListBucketsCommand.
  /// 
  /// </summary>
  internal class ListBucketsCommand : OssCommand<IEnumerable<Bucket>>
  {
    public ListBucketsCommand(IServiceClient client, Uri endpoint, ExecutionContext context, IDeserializer<ServiceResponse, IEnumerable<Bucket>> deserializeMethod)
      : base(client, endpoint, context, deserializeMethod)
    {
    }

    public static ListBucketsCommand Create(IServiceClient client, Uri endpoint, ExecutionContext context)
    {
      return new ListBucketsCommand(client, endpoint, context, DeserializerFactory.GetFactory().CreateListBucketResultDeserializer());
    }
  }
}
