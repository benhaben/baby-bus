// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.Utilities.OtsUtility
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using Aliyun.OpenServices.OpenTableService;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
  /// <summary>
  /// Helper class.
  /// 
  /// </summary>
  internal static class OtsUtility
  {
    public static readonly Encoding DataEncoding = Encoding.UTF8;

    public static bool IsEntityNameValid(string name)
    {
      Debug.Assert(!string.IsNullOrEmpty(name));
      return new Regex("^[a-zA-Z_][\\w]{0,99}$").Match(name).Success;
    }

    /// <summary>
    /// Gets the TableName parameter for GetRow(xYz).
    ///             If ViewName is set, returns the full view name. Otherwise, returns the TableName.
    /// 
    /// </summary>
    /// <param name="criteria"/>
    /// <returns/>
    public static string GetFullQueryTableName(RowQueryCriteria criteria)
    {
      Debug.Assert(criteria != null);
      Debug.Assert(!string.IsNullOrEmpty(criteria.TableName) && OtsUtility.IsEntityNameValid(criteria.TableName));
      string str = criteria.TableName;
      if (!string.IsNullOrEmpty(criteria.ViewName))
      {
        Debug.Assert(OtsUtility.IsEntityNameValid(criteria.ViewName));
        str = str + "." + criteria.ViewName;
      }
      return str;
    }

    [Conditional("DEBUG")]
    [DebuggerNonUserCode]
    public static void AssertColumnNames(IEnumerable<string> names)
    {
      foreach (string name in names)
        Debug.Assert(OtsUtility.IsEntityNameValid(name));
    }
  }
}
