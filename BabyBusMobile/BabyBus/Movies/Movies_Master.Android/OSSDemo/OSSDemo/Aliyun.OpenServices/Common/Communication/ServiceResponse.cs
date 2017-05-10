// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.ServiceResponse
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Diagnostics;
using System.Net;

namespace Aliyun.OpenServices.Common.Communication
{
  /// <summary>
  /// Represents the data of the responses of requests.
  /// 
  /// </summary>
  internal abstract class ServiceResponse : ServiceMessage, IDisposable
  {
    public abstract HttpStatusCode StatusCode { get; }

    public abstract Exception Failure { get; }

    public virtual bool IsSuccessful()
    {
      return (int) this.StatusCode / 100 == 2;
    }

    /// <summary>
    /// Throws the exception from communication if the status code is not 2xx.
    /// 
    /// </summary>
    public virtual void EnsureSuccessful()
    {
      if (this.IsSuccessful())
        return;
      if (this.Content != null)
        this.Content.Dispose();
      Debug.Assert(this.Failure != null);
      throw this.Failure;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
  }
}
