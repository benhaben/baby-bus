// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.TableValueExtension
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  internal static class TableValueExtension
  {
    internal static string ToParameterString(this PrimaryKeyValue value)
    {
      switch (value.ValueType)
      {
        case PrimaryKeyType.String:
          if (value.IsInf)
            return value.ToString();
          else
            return "'" + value.ToString() + "'";
        case PrimaryKeyType.Boolean:
          return value.Value.ToUpperInvariant();
        default:
          return value.Value;
      }
    }

    internal static string ToParameterString(this ColumnValue value)
    {
      switch (value.ValueType)
      {
        case ColumnType.String:
          return "'" + value.ToString() + "'";
        case ColumnType.Boolean:
          return value.Value.ToUpperInvariant();
        default:
          return value.Value;
      }
    }

    internal static string ToParameterString(this PartitionKeyValue value)
    {
      if (value.ValueType == PartitionKeyType.String)
        return "'" + value.ToString() + "'";
      else
        return value.Value;
    }
  }
}
