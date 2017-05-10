// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.ExecutionContext
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Handlers;
using System.Collections.Generic;

namespace Aliyun.OpenServices.Common.Communication
{
  /// <summary>
  /// Description of ExecutionContext.
  /// 
  /// </summary>
  internal class ExecutionContext
  {
    private IList<IResponseHandler> _responseHandlers = (IList<IResponseHandler>) new List<IResponseHandler>();
    /// <summary>
    /// The default encoding (charset name).
    /// 
    /// </summary>
    private const string DefaultEncoding = "utf-8";

    /// <summary>
    /// Gets or sets the charset.
    /// 
    /// </summary>
    public string Charset { get; set; }

    /// <summary>
    /// Gets or sets the request signer.
    /// 
    /// </summary>
    public IRequestSigner Signer { get; set; }

    /// <summary>
    /// Gets or sets the credentials.
    /// 
    /// </summary>
    public ServiceCredentials Credentials { get; set; }

    /// <summary>
    /// Gets the list of <see cref="T:Aliyun.OpenServices.Common.Handlers.IResponseHandler"/>.
    /// 
    /// </summary>
    public IList<IResponseHandler> ResponseHandlers
    {
      get
      {
        return this._responseHandlers;
      }
    }

    /// <summary>
    /// Constructor.
    /// 
    /// </summary>
    public ExecutionContext()
    {
      this.Charset = "utf-8";
    }
  }
}
