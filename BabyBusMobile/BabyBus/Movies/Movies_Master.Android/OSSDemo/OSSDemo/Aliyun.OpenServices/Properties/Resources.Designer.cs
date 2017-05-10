// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.Properties.Resources
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Aliyun.OpenServices.Properties
{
  /// <summary>
  /// A strongly-typed resource class, for looking up localized strings, etc.
  /// 
  /// </summary>
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    /// <summary>
    /// Returns the cached ResourceManager instance used by this class.
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Resources.resourceMan, (object) null))
          Resources.resourceMan = new ResourceManager("Aliyun.OpenServices.Properties.Resources", typeof (Resources).Assembly);
        return Resources.resourceMan;
      }
    }

    /// <summary>
    /// Overrides the current thread's CurrentUICulture property for all
    ///               resource lookups using this strongly typed resource class.
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Resources.resourceCulture;
      }
      set
      {
        Resources.resourceCulture = value;
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 以前使用 asyncResult调用过此方法。.
    /// 
    /// </summary>
    internal static string ExceptionEndOperationHasBeenCalled
    {
      get
      {
        return Resources.ResourceManager.GetString("ExceptionEndOperationHasBeenCalled", Resources.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 参数为空引用或值为长度是零的字符串。.
    /// 
    /// </summary>
    internal static string ExceptionIfArgumentStringIsNullOrEmpty
    {
      get
      {
        return Resources.ResourceManager.GetString("ExceptionIfArgumentStringIsNullOrEmpty", Resources.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 返回结果无法解析。.
    /// 
    /// </summary>
    internal static string ExceptionInvalidResponse
    {
      get
      {
        return Resources.ResourceManager.GetString("ExceptionInvalidResponse", Resources.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 服务器返回未知错误。.
    /// 
    /// </summary>
    internal static string ExceptionUnknowError
    {
      get
      {
        return Resources.ResourceManager.GetString("ExceptionUnknowError", Resources.resourceCulture);
      }
    }

    internal Resources()
    {
    }
  }
}
