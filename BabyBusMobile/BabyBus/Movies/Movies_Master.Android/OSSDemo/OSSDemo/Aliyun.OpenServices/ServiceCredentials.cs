// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.ServiceCredentials
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices
{
  /// <summary>
  /// Represents the credentials used to access Aliyun Open Services.
  /// 
  /// </summary>
  internal class ServiceCredentials
  {
    /// <summary>
    /// Gets the access ID.
    /// 
    /// </summary>
    public string AccessId { get; private set; }

    /// <summary>
    /// Gets the access key.
    /// 
    /// </summary>
    public string AccessKey { get; private set; }

    /// <summary>
    /// Initialize an new instance of <see cref="T:Aliyun.OpenServices.ServiceCredentials"/>.
    /// 
    /// </summary>
    /// <param name="accessId">The access ID.</param><param name="accessKey">The access key.</param>
    public ServiceCredentials(string accessId, string accessKey)
    {
      if (string.IsNullOrEmpty(accessId))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
      this.AccessId = accessId;
      this.AccessKey = accessKey;
    }
  }
}
