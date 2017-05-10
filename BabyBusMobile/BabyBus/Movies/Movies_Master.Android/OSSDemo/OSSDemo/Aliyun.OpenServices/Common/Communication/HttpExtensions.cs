// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Communication.HttpExtensions
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Aliyun.OpenServices.Common.Communication
{
  internal static class HttpExtensions
  {
    private static ICollection<PlatformID> monoPlatforms = (ICollection<PlatformID>) new List<PlatformID>()
    {
      PlatformID.MacOSX,
      PlatformID.Unix
    };
    private static MethodInfo _addInternalMethod;
    private static bool? isMonoPlatform;

    internal static void AddInternal(this WebHeaderCollection headers, string key, string value)
    {
      if (!HttpExtensions.isMonoPlatform.HasValue)
        HttpExtensions.isMonoPlatform = new bool?(HttpExtensions.monoPlatforms.Contains(Environment.OSVersion.Platform));
      bool? nullable = HttpExtensions.isMonoPlatform;
      if ((nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        value = HttpUtils.ReEncode(value, "utf-8", "iso-8859-1");
      if (HttpExtensions._addInternalMethod == null)
      {
        nullable = HttpExtensions.isMonoPlatform;
        HttpExtensions._addInternalMethod = typeof (WebHeaderCollection).GetMethod((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? "AddWithoutValidate" : "AddInternal", BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (string),
          typeof (string)
        }, (ParameterModifier[]) null);
      }
      HttpExtensions._addInternalMethod.Invoke((object) headers, new object[2]
      {
        (object) key,
        (object) value
      });
    }
  }
}
