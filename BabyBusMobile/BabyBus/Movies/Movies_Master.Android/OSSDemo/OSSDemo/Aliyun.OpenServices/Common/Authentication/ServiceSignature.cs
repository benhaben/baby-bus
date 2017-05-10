// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Authentication.ServiceSignature
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Properties;
using System;

namespace Aliyun.OpenServices.Common.Authentication
{
  internal abstract class ServiceSignature
  {
    public abstract string SignatureMethod { get; }

    public abstract string SignatureVersion { get; }

    public string ComputeSignature(string key, string data)
    {
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
      if (string.IsNullOrEmpty(data))
        throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "data");
      else
        return this.ComputeSignatureCore(key, data);
    }

    protected abstract string ComputeSignatureCore(string key, string data);

    public static ServiceSignature Create()
    {
      return (ServiceSignature) new HmacSHA1Signature();
    }
  }
}
