// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.ColumnTypeHelper
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Helper for <see cref="T:Aliyun.OpenServices.OpenTableService.ColumnType"/>.
  /// 
  /// </summary>
  internal static class ColumnTypeHelper
  {
    private static object _lockObj = new object();
    private static EnumTypeStringMap<ColumnType> _innerMap;

    public static string GetString(ColumnType pkType)
    {
      ColumnTypeHelper.EnsureMapCreated();
      return ColumnTypeHelper._innerMap.GetEnumString(pkType);
    }

    public static ColumnType Parse(string value)
    {
      ColumnTypeHelper.EnsureMapCreated();
      return ColumnTypeHelper._innerMap.GetEnumType(value);
    }

    private static void EnsureMapCreated()
    {
      if (ColumnTypeHelper._innerMap != null)
        return;
      EnumTypeStringMap<ColumnType> enumTypeStringMap = new EnumTypeStringMap<ColumnType>((IDictionary<ColumnType, string>) new Dictionary<ColumnType, string>()
      {
        {
          ColumnType.String,
          "STRING"
        },
        {
          ColumnType.Integer,
          "INTEGER"
        },
        {
          ColumnType.Boolean,
          "BOOLEAN"
        },
        {
          ColumnType.Double,
          "DOUBLE"
        }
      });
      lock (ColumnTypeHelper._lockObj)
      {
        if (ColumnTypeHelper._innerMap == null)
          ColumnTypeHelper._innerMap = enumTypeStringMap;
      }
    }
  }
}
