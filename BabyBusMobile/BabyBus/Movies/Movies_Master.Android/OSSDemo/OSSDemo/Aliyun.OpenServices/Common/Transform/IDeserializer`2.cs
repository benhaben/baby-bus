// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Transform.IDeserializer`2
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

namespace Aliyun.OpenServices.Common.Transform
{
  /// <summary>
  /// Description of Deserializer.
  /// 
  /// </summary>
  internal interface IDeserializer<TInput, TOutput>
  {
    /// <summary>
    /// Deserialize the instance <typeparamref name="TOutput"/>
    ///             from an instance of <typeparamref name="TInput"/>
    /// </summary>
    /// <param name="input"/>
    /// <returns/>
    /// <exception cref="T:Aliyun.OpenServices.Common.Transform.ResponseDeserializationException">Failed to deserialize the response.</exception>
    TOutput Deserialize(TInput input);
  }
}
