// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.AsyncResult
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Diagnostics;
using System.Threading;

namespace Aliyun.OpenServices.Common
{
  /// <summary>
  /// The implementation of <see cref="T:System.IAsyncResult"/>
  ///             that represents the status of an async operation.
  /// 
  /// </summary>
  internal abstract class AsyncResult : IAsyncResult, IDisposable
  {
    private object _asyncState;
    private bool _completedSynchronously;
    private bool _isCompleted;
    private AsyncCallback _userCallback;
    private ManualResetEvent _asyncWaitEvent;
    private Exception _exception;

    /// <summary>
    /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
    /// 
    /// </summary>
    [DebuggerNonUserCode]
    public object AsyncState
    {
      get
      {
        return this._asyncState;
      }
    }

    /// <summary>
    /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
    /// 
    /// </summary>
    [DebuggerNonUserCode]
    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this._asyncWaitEvent != null)
          return (WaitHandle) this._asyncWaitEvent;
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        if (Interlocked.CompareExchange<ManualResetEvent>(ref this._asyncWaitEvent, manualResetEvent, (ManualResetEvent) null) != null)
          manualResetEvent.Close();
        if (this.IsCompleted)
          this._asyncWaitEvent.Set();
        return (WaitHandle) this._asyncWaitEvent;
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the asynchronous operation completed synchronously.
    /// 
    /// </summary>
    [DebuggerNonUserCode]
    public bool CompletedSynchronously
    {
      get
      {
        return this._completedSynchronously;
      }
      protected set
      {
        this._completedSynchronously = value;
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the asynchronous operation has completed.
    /// 
    /// </summary>
    [DebuggerNonUserCode]
    public bool IsCompleted
    {
      get
      {
        return this._isCompleted;
      }
    }

    /// <summary>
    /// Initializes an instance of <see cref="T:Aliyun.OpenServices.Common.AsyncResult"/>.
    /// 
    /// </summary>
    /// <param name="callback">The callback method when the async operation completes.</param><param name="state">A user-defined object that qualifies or contains information about an asynchronous operation.</param>
    protected AsyncResult(AsyncCallback callback, object state)
    {
      this._userCallback = callback;
      this._asyncState = state;
    }

    /// <summary>
    /// Completes the async operation with an exception.
    /// 
    /// </summary>
    /// <param name="ex">Exception from the async operation.</param>
    public void Complete(Exception ex)
    {
      Debug.Assert(ex != null);
      this._exception = ex;
      this.NotifyCompletion();
    }

    /// <summary>
    /// When called in the dervied classes, wait for completion.
    ///             It throws exception if the async operation ends with an exception.
    /// 
    /// </summary>
    protected void WaitForCompletion()
    {
      if (!this.IsCompleted)
        this._asyncWaitEvent.WaitOne();
      if (this._exception != null)
        throw this._exception;
    }

    /// <summary>
    /// When called in the derived classes, notify operation completion
    ///             by setting <see cref="P:AsyncWaitHandle"/> and calling the user callback.
    /// 
    /// </summary>
    [DebuggerNonUserCode]
    protected void NotifyCompletion()
    {
      this._isCompleted = true;
      if (this._asyncWaitEvent != null)
        this._asyncWaitEvent.Set();
      if (this._userCallback == null)
        return;
      this._userCallback((IAsyncResult) this);
    }

    /// <summary>
    /// Disposes the object and release resource.
    /// 
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// When overrided in the derived classes, release resources.
    /// 
    /// </summary>
    /// <param name="disposing">Whether the method is called <see cref="M:Dispose"/></param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._asyncWaitEvent == null)
        return;
      this._asyncWaitEvent.Close();
      this._asyncWaitEvent = (ManualResetEvent) null;
    }
  }
}
