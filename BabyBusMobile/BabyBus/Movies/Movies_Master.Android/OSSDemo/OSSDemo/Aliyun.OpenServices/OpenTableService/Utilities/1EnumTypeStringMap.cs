// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.PartitionKeyTypeHelper
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Helper for <see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyType"/>.
  /// 
  /// </summary>
  internal static class PartitionKeyTypeHelper
  {
    private static object _lockObj = new object();
    private static EnumTypeStringMap<PartitionKeyType> _innerMap;

    public static string GetString(PartitionKeyType pkType)
    {
      PartitionKeyTypeHelper.EnsureMapCreated();
      return PartitionKeyTypeHelper._innerMap.GetEnumString(pkType);
    }

    public static PartitionKeyType Parse(string value)
    {
      PartitionKeyTypeHelper.EnsureMapCreated();
      return PartitionKeyTypeHelper._innerMap.GetEnumType(value);
    }

    private static void EnsureMapCreated()
    {
      if (PartitionKeyTypeHelper._innerMap != null)
        return;
      EnumTypeStringMap<PartitionKeyType> enumTypeStringMap = new EnumTypeStringMap<PartitionKeyType>((IDictionary<PartitionKeyType, string>) new Dictionary<PartitionKeyType, string>()
      {
        {
          PartitionKeyType.String,
          "STRING"
        },
        {
          PartitionKeyType.Integer,
          "INTEGER"
        }
      });
      lock (PartitionKeyTypeHelper._lockObj)
      {
        if (PartitionKeyTypeHelper._innerMap == null)
          PartitionKeyTypeHelper._innerMap = enumTypeStringMap;
      }
    }
  }
}
