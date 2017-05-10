// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Transform.XmlStreamDeserializer`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.Common.Transform
{
  /// <summary>
  /// XmlStreamDeserializer.
  ///             It deserializes the object from XML stream.
  /// 
  /// </summary>
  internal class XmlStreamDeserializer<T> : IDeserializer<Stream, T>
  {
    private static readonly XmlSerializer _serializer = new XmlSerializer(typeof (T));

    /// <summary>
    /// Deserialize the result to an object of T from the <see cref="T:Aliyun.OpenServices.Common.Communication.ServiceResponse"/>.
    ///             It will close the underlying stream.
    /// 
    /// </summary>
    /// <param name="xml"/>
    /// <returns/>
    public T Deserialize(Stream xml)
    {
      Debug.Assert(xml != null);
      using (xml)
      {
        try
        {
          return (T) XmlStreamDeserializer<T>._serializer.Deserialize(xml);
        }
        catch (XmlException ex)
        {
          throw new ResponseDeserializationException(ex.Message, (Exception) ex);
        }
        catch (InvalidOperationException ex)
        {
          throw new ResponseDeserializationException(ex.Message, (Exception) ex);
        }
      }
    }
  }
}
