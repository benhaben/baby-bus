// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.PrimaryKeyTypeHelper
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Helper for <see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyType"/>.
  /// 
  /// </summary>
  internal static class PrimaryKeyTypeHelper
  {
    private static object _lockObj = new object();
    private static EnumTypeStringMap<PrimaryKeyType> _innerMap;

    public static string GetString(PrimaryKeyType pkType)
    {
      PrimaryKeyTypeHelper.EnsureMapCreated();
      return PrimaryKeyTypeHelper._innerMap.GetEnumString(pkType);
    }

    public static PrimaryKeyType Parse(string value)
    {
      PrimaryKeyTypeHelper.EnsureMapCreated();
      return PrimaryKeyTypeHelper._innerMap.GetEnumType(value);
    }

    private static void EnsureMapCreated()
    {
      if (PrimaryKeyTypeHelper._innerMap != null)
        return;
      EnumTypeStringMap<PrimaryKeyType> enumTypeStringMap = new EnumTypeStringMap<PrimaryKeyType>((IDictionary<PrimaryKeyType, string>) new Dictionary<PrimaryKeyType, string>()
      {
        {
          PrimaryKeyType.String,
          "STRING"
        },
        {
          PrimaryKeyType.Integer,
          "INTEGER"
        },
        {
          PrimaryKeyType.Boolean,
          "BOOLEAN"
        }
      });
      lock (PrimaryKeyTypeHelper._lockObj)
      {
        if (PrimaryKeyTypeHelper._innerMap == null)
          PrimaryKeyTypeHelper._innerMap = enumTypeStringMap;
      }
    }
  }
}
