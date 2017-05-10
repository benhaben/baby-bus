// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.EnumTypeStringMap`1
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// A mapping that maps enum fields to their string representations.
  /// 
  /// </summary>
  /// <typeparam name="T"/>
  internal class EnumTypeStringMap<T>
  {
    private IDictionary<string, T> _stringToTypeMap;
    private IDictionary<T, string> _typeToStringMap;

    public EnumTypeStringMap(IDictionary<T, string> typeToStringMap)
    {
      Debug.Assert(typeToStringMap != null);
      this._typeToStringMap = typeToStringMap;
      this._stringToTypeMap = (IDictionary<string, T>) new Dictionary<string, T>();
      foreach (KeyValuePair<T, string> keyValuePair in (IEnumerable<KeyValuePair<T, string>>) typeToStringMap)
        this._stringToTypeMap.Add(keyValuePair.Value, keyValuePair.Key);
      this.CheckMap();
    }

    [DebuggerNonUserCode]
    [Conditional("DEBUG")]
    private void CheckMap()
    {
      Type type = typeof (T);
      Debug.Assert(type.IsEnum);
      foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
        Debug.Assert(Enumerable.Contains<string>(Enumerable.Select<T, string>((IEnumerable<T>) this._typeToStringMap.Keys, (Func<T, string>) (k => k.ToString())), fieldInfo.Name));
    }

    public T GetEnumType(string value)
    {
      Debug.Assert(!string.IsNullOrEmpty(value));
      Debug.Assert(this._stringToTypeMap.Keys.Contains(value));
      if (this._stringToTypeMap.Keys.Contains(value))
        return this._stringToTypeMap[value];
      else
        return default (T);
    }

    public string GetEnumString(T value)
    {
      Debug.Assert(this._typeToStringMap.Keys.Contains(value));
      return this._typeToStringMap[value];
    }
  }
}
