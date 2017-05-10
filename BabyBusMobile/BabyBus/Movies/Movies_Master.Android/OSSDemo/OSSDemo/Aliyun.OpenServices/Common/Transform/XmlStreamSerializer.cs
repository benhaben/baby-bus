// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Transform.XmlStreamSerializer`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.IO;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.Common.Transform
{
  /// <summary>
  /// XmlSerializer.
  ///             It serializes objects to XML content.
  /// 
  /// </summary>
  internal class XmlStreamSerializer<T> : ISerializer<T, Stream>
  {
    private static readonly XmlSerializer _serializer = new XmlSerializer(typeof (T));

    public Stream Serialize(T obj)
    {
      Stream stream = (Stream) new MemoryStream();
      try
      {
        XmlStreamSerializer<T>._serializer.Serialize(stream, (object) obj);
        stream.Seek(0L, SeekOrigin.Begin);
        return stream;
      }
      catch (InvalidOperationException ex)
      {
        stream.Close();
        throw new RequestSerializationException(ex.Message, (Exception) ex);
      }
    }
  }
}
