// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.AsyncResult`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;

namespace Aliyun.OpenServices.Common
{
  /// <summary>
  /// Represents the status of an async operation.
  ///             It also holds the result of the operation.
  /// 
  /// </summary>
  /// <typeparam name="T">Type of the operation result.</typeparam>
  internal class AsyncResult<T> : AsyncResult
  {
    /// <summary>
    /// The result of the async operation.
    /// 
    /// </summary>
    private T _result;

    /// <summary>
    /// Initializes an instance of <see cref="T:Aliyun.OpenServices.Common.AsyncResult`1"/>.
    /// 
    /// </summary>
    /// <param name="callback">The callback method when the async operation completes.</param><param name="state">A user-defined object that qualifies or contains information about an asynchronous operation.</param>
    public AsyncResult(AsyncCallback callback, object state)
      : base(callback, state)
    {
    }

    /// <summary>
    /// Gets result and release resources.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The instance of result.
    /// </returns>
    public T GetResult()
    {
      this.WaitForCompletion();
      return this._result;
    }

    /// <summary>
    /// Sets result and notify completion.
    /// 
    /// </summary>
    /// <param name="result">The instance of result.</param>
    public void Complete(T result)
    {
      this._result = result;
      this.NotifyCompletion();
    }
  }
}
