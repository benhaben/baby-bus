// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Transform.ResponseDeserializationException
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Runtime.Serialization;

namespace Aliyun.OpenServices.Common.Transform
{
  /// <summary>
  /// Exception thrown during deserializing the response.
  /// 
  /// </summary>
  [Serializable]
  internal class ResponseDeserializationException : InvalidOperationException, ISerializable
  {
    public ResponseDeserializationException()
    {
    }

    public ResponseDeserializationException(string message)
      : base(message)
    {
    }

    public ResponseDeserializationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ResponseDeserializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
