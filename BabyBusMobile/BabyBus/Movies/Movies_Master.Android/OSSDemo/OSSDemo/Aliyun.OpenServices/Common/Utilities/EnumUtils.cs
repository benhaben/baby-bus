// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Common.Utilities.EnumUtils
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.Common.Utilities
{
  /// <summary>
  /// Description of EnumUtils.
  /// 
  /// </summary>
  internal static class EnumUtils
  {
    private static IDictionary<Enum, StringValueAttribute> _stringValues = (IDictionary<Enum, StringValueAttribute>) new Dictionary<Enum, StringValueAttribute>();

    public static string GetStringValue(this Enum value)
    {
      Type type = value.GetType();
      string str;
      if (EnumUtils._stringValues.ContainsKey(value))
      {
        str = EnumUtils._stringValues[value].Value;
      }
      else
      {
        StringValueAttribute[] stringValueAttributeArray = type.GetField(((object) value).ToString()).GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[];
        if (stringValueAttributeArray.Length <= 0)
          return ((object) value).ToString();
        str = stringValueAttributeArray[0].Value;
        lock (EnumUtils._stringValues)
        {
          if (!EnumUtils._stringValues.ContainsKey(value))
            EnumUtils._stringValues.Add(value, stringValueAttributeArray[0]);
        }
      }
      return str;
    }
  }
}
