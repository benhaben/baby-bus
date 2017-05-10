// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.ServiceClientFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService;
using System;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Description of ServiceClientFactory.
  /// 
  /// </summary>
  internal class ServiceClientFactory
  {
    public static IServiceClient CreateServiceClient(ClientConfiguration configuration)
    {
      Debug.Assert(configuration != null);
      return (IServiceClient) new RetryableServiceClient((IServiceClient) ServiceClient.Create(configuration))
      {
        MaxErrorRetry = configuration.MaxErrorRetry,
        ShouldRetryCallback = new Func<Exception, bool>(ServiceClientFactory.CanRetry)
      };
    }

    private static bool CanRetry(Exception ex)
    {
      OtsException otsException = ex as OtsException;
      if (otsException != null)
        return otsException.ErrorCode == "OTSInternalServerError" || otsException.ErrorCode == "OTSStorageServerBusy" || (otsException.ErrorCode == "OTSStorageTimeout" || otsException.ErrorCode == "OTSStorageTxnLockKeyFail") || otsException.ErrorCode == "OTSStoragePartitionNotReady";
      else
        return false;
    }
  }
}
