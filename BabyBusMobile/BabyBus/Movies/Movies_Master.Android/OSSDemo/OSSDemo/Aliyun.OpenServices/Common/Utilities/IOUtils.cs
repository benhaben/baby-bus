// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Utilities.IOUtils
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.IO;

namespace Aliyun.OpenServices.Common.Utilities
{
  /// <summary>
  /// Description of IOUtils.
  /// 
  /// </summary>
  internal static class IOUtils
  {
    private const int _bufferSize = 4096;

    public static void WriteTo(this Stream src, Stream dest)
    {
      if (dest == null)
        throw new ArgumentNullException("dest");
      byte[] buffer = new byte[4096];
      int count;
      while ((count = src.Read(buffer, 0, buffer.Length)) > 0)
        dest.Write(buffer, 0, count);
      dest.Flush();
    }

    /// <summary>
    /// Write a stream to another
    /// 
    /// </summary>
    /// <param name="orignStream">The stream you want to write from</param><param name="destStream">The stream written to</param><param name="maxLength">The max length of the stream to write</param>
    /// <returns>
    /// The actual length written to destStream
    /// </returns>
    public static long WriteTo(this Stream orignStream, Stream destStream, long maxLength)
    {
      byte[] buffer = new byte[1024];
      long num = 0L;
      while (num < maxLength)
      {
        int count = orignStream.Read(buffer, 0, 1024);
        if (count > 0)
        {
          if (num + (long) count > maxLength)
            count = (int) (maxLength - num);
          num += (long) count;
          destStream.Write(buffer, 0, count);
        }
        else
          break;
      }
      destStream.Flush();
      return num;
    }
  }
}
