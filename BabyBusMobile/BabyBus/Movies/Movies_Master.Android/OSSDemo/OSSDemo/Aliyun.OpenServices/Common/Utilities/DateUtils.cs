// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Utilities.DateUtils
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.Common.Utilities
{
  /// <summary>
  /// Description of DateUtils.
  /// 
  /// </summary>
  internal static class DateUtils
  {
    private const string _rfc822DateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";
    private const string _iso8601DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";

    /// <summary>
    /// Formats an instance of <see cref="T:System.DateTime"/> to a GMT string.
    /// 
    /// </summary>
    /// <param name="dt">The date time to format.</param>
    /// <returns/>
    public static string FormatRfc822Date(DateTime dt)
    {
      return dt.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats a GMT date string to an object of <see cref="T:System.DateTime"/>.
    /// 
    /// </summary>
    /// <param name="dt"/>
    /// <returns/>
    public static DateTime ParseRfc822Date(string dt)
    {
      Debug.Assert(!string.IsNullOrEmpty(dt));
      return DateTime.SpecifyKind(DateTime.ParseExact(dt, "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T", (IFormatProvider) CultureInfo.InvariantCulture), DateTimeKind.Utc);
    }

    /// <summary>
    /// Formats a date to a string in the format of ISO 8601 spec.
    /// 
    /// </summary>
    /// <param name="dt"/>
    /// <returns/>
    public static string FormatIso8601Date(DateTime dt)
    {
      return dt.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", (IFormatProvider) CultureInfo.CreateSpecificCulture("en-US"));
    }
  }
}
