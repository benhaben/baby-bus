// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.Commands.OssCommand
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.OpenStorageService.Commands
{
  /// <summary>
  /// Base class for OSS Commands.
  /// 
  /// </summary>
  internal abstract class OssCommand
  {
    public ExecutionContext Context { get; private set; }

    public IServiceClient Client { get; private set; }

    public Uri Endpoint { get; private set; }

    protected virtual bool LeaveRequestOpen
    {
      get
      {
        return false;
      }
    }

    protected virtual HttpMethod Method
    {
      get
      {
        return HttpMethod.Get;
      }
    }

    protected virtual string Bucket
    {
      get
      {
        return (string) null;
      }
    }

    protected virtual string Key
    {
      get
      {
        return (string) null;
      }
    }

    protected virtual IDictionary<string, string> Headers
    {
      get
      {
        return (IDictionary<string, string>) new Dictionary<string, string>();
      }
    }

    protected virtual IDictionary<string, string> Parameters
    {
      get
      {
        return (IDictionary<string, string>) new Dictionary<string, string>();
      }
    }

    protected virtual Stream Content
    {
      get
      {
        return (Stream) null;
      }
    }

    public OssCommand(IServiceClient client, Uri ossEndpoint, ExecutionContext context)
    {
      Debug.Assert(client != null && ossEndpoint != (Uri) null && context != null);
      this.Endpoint = ossEndpoint;
      this.Client = client;
      this.Context = context;
    }

    public ServiceResponse Execute()
    {
      ServiceRequest request = this.BuildRequest();
      try
      {
        return this.Client.Send(request, this.Context);
      }
      finally
      {
        if (!this.LeaveRequestOpen)
          request.Dispose();
      }
    }

    private ServiceRequest BuildRequest()
    {
      ServiceRequest serviceRequest = new ServiceRequest();
      serviceRequest.Method = this.Method;
      serviceRequest.Endpoint = OssUtils.MakeBucketEndpoint(this.Endpoint, this.Bucket);
      serviceRequest.ResourcePath = OssUtils.MakeResourcePath(this.Key);
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.Parameters)
        serviceRequest.Parameters.Add(keyValuePair.Key, keyValuePair.Value);
      serviceRequest.Headers["Date"] = DateUtils.FormatRfc822Date(DateTime.UtcNow);
      if (!this.Headers.ContainsKey("Content-Type"))
        serviceRequest.Headers["Content-Type"] = string.Empty;
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.Headers)
        serviceRequest.Headers.Add(keyValuePair.Key, keyValuePair.Value);
      serviceRequest.Content = this.Content;
      return serviceRequest;
    }
  }
}
