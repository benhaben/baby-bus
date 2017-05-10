// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Utilities.ExecutionContextBuilder
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Handlers;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService.Utilities
{
  internal class ExecutionContextBuilder
  {
    public ServiceCredentials Credentials { get; set; }

    public IList<IResponseHandler> ResponseHandlers { get; private set; }

    public HttpMethod Method { get; set; }

    public string Bucket { get; set; }

    public string Key { get; set; }

    public ExecutionContextBuilder()
    {
      this.ResponseHandlers = (IList<IResponseHandler>) new List<IResponseHandler>();
    }

    public ExecutionContext Build()
    {
      ExecutionContext executionContext = new ExecutionContext();
      executionContext.Signer = ExecutionContextBuilder.CreateSigner(this.Bucket, this.Key);
      executionContext.Credentials = this.Credentials;
      foreach (IResponseHandler responseHandler in (IEnumerable<IResponseHandler>) this.ResponseHandlers)
        executionContext.ResponseHandlers.Add(responseHandler);
      return executionContext;
    }

    private static IRequestSigner CreateSigner(string bucket, string key)
    {
      string resourcePath = "/" + (bucket != null ? bucket : "") + (key != null ? "/" + key : "");
      if (bucket != null && key == null)
        resourcePath = resourcePath + "/";
      return (IRequestSigner) new OssRequestSigner(resourcePath);
    }
  }
}
