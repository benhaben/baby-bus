// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Authentication.IRequestSigner
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.Common.Authentication
{
  /// <summary>
  /// Description of IRequestSigner.
  /// 
  /// </summary>
  internal interface IRequestSigner
  {
    /// <summary>
    /// Signs a request.
    /// 
    /// </summary>
    /// <param name="request">The request to sign.</param><param name="credentials">The credentials used to sign.</param>
    void Sign(ServiceRequest request, ServiceCredentials credentials);
  }
}
