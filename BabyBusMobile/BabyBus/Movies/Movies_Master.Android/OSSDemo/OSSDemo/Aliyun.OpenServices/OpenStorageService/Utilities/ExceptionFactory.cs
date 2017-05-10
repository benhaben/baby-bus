// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Utilities.ExceptionFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenStorageService;
using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
  /// <summary>
  /// Description of ExceptionFactory.
  /// 
  /// </summary>
  internal static class ExceptionFactory
  {
    public static OssException CreateException(string errorCode, string message, string requestId, string hostId)
    {
      return ExceptionFactory.CreateException(errorCode, message, requestId, hostId, (Exception) null);
    }

    public static OssException CreateException(string errorCode, string message, string requestId, string hostId, Exception innerException)
    {
      OssException ossException = innerException != null ? new OssException(message, innerException) : new OssException(message);
      ossException.RequestId = requestId;
      ossException.HostId = hostId;
      ossException.ErrorCode = errorCode;
      return ossException;
    }

    public static Exception CreateInvalidResponseException(Exception innerException)
    {
      throw new InvalidOperationException(Resources.ExceptionInvalidResponse, innerException);
    }
  }
}
