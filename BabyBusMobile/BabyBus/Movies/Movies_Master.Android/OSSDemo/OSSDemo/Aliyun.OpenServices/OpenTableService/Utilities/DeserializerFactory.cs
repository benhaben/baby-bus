// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.DeserializerFactory
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Transform;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// The factory to create deserialization instances.
  /// 
  /// </summary>
  internal static class DeserializerFactory
  {
    public static IDeserializer<Stream, T> CreateDeserializer<T>(string contentType)
    {
      if (contentType == null)
        contentType = "text/xml";
      if (contentType.Contains("xml"))
        return (IDeserializer<Stream, T>) new XmlStreamDeserializer<T>();
      else
        return (IDeserializer<Stream, T>) null;
    }
  }
}
