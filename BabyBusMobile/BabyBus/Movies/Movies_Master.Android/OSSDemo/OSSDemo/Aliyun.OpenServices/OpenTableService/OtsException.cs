// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.OtsException
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 该异常在对开放结构化数据服务（Open Table Service）访问失败时抛出。
  /// 
  /// </summary>
  /// <seealso cref="T:Aliyun.OpenServices.ServiceException"/>
  [Serializable]
  public class OtsException : ServiceException
  {
    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.OtsException"/>实例。
    /// 
    /// </summary>
    public OtsException()
    {
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.OtsException"/>实例。
    /// 
    /// </summary>
    /// <param name="message">解释异常原因的错误信息。</param>
    public OtsException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.OtsException"/>实例。
    /// 
    /// </summary>
    /// <param name="info">保存序列化对象数据的对象。</param><param name="context">有关源或目标的上下文信息。</param>
    protected OtsException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.OtsException"/>实例。
    /// 
    /// </summary>
    /// <param name="message">解释异常原因的错误信息。</param><param name="innerException">导致当前异常的异常。</param>
    public OtsException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    /// 重载<see cref="M:System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"/>方法。
    /// 
    /// </summary>
    /// <param name="info"><see cref="T:System.Runtime.Serialization.SerializationInfo"/>，它存有有关所引发异常的序列化的对象数据。</param><param name="context"><see cref="T:System.Runtime.Serialization.StreamingContext"/>，它包含有关源或目标的上下文信息。</param>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
    }
  }
}
