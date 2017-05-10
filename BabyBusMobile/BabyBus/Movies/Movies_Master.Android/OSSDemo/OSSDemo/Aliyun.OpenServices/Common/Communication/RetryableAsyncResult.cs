// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.RetryableAsyncResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common;
using System;
using System.Diagnostics;

namespace Aliyun.OpenServices.Common.Communication
{
  internal class RetryableAsyncResult : AsyncResult<ServiceResponse>
  {
    public ServiceRequest Request { get; private set; }

    public ExecutionContext Context { get; private set; }

    public AsyncResult InnerAsyncResult { get; set; }

    public int Retries { get; set; }

    public long OriginalContentPosition { get; set; }

    public RetryableAsyncResult(AsyncCallback callback, object state, ServiceRequest request, ExecutionContext context)
      : base(callback, state)
    {
      Debug.Assert(request != null);
      this.Request = request;
      this.Context = context;
      this.OriginalContentPosition = request.Content == null || !request.Content.CanSeek ? -1L : request.Content.Position;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing || this.InnerAsyncResult == null)
        return;
      this.InnerAsyncResult.Dispose();
      this.InnerAsyncResult = (AsyncResult) null;
    }
  }
}
