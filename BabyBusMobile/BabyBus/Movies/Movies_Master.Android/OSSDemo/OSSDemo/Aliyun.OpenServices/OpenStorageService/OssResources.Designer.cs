// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenStorageService.OssResources
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Aliyun.OpenServices.OpenStorageService
{
  /// <summary>
  /// A strongly-typed resource class, for looking up localized strings, etc.
  /// 
  /// </summary>
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal class OssResources
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
        if (object.ReferenceEquals((object) OssResources.resourceMan, (object) null))
          OssResources.resourceMan = new ResourceManager("Aliyun.OpenServices.OpenStorageService.OssResources", typeof (OssResources).Assembly);
        return OssResources.resourceMan;
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
        return OssResources.resourceCulture;
      }
      set
      {
        OssResources.resourceCulture = value;
      }
    }

    /// <summary>
    /// Looks up a localized string similar to Bucket名称无效。Bucket 命名规范：
    ///             1)只能包括小写字母，数字，下划线（_）和短横线（-）；
    ///             2)必须以小写字母或者数字开头；
    ///             3)长度必须在 3-255 字节之间。.
    /// 
    /// </summary>
    internal static string BucketNameInvalid
    {
      get
      {
        return OssResources.ResourceManager.GetString("BucketNameInvalid", OssResources.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 不支持终结点给定的协议。仅支持HTTP协议，即终结点必须以"http://"开头。.
    /// 
    /// </summary>
    internal static string EndpointNotSupportedProtocal
    {
      get
      {
        return OssResources.ResourceManager.GetString("EndpointNotSupportedProtocal", OssResources.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to Object Key无效，长度必须大于0且不超过1023个字节。.
    /// 
    /// </summary>
    internal static string ObjectKeyInvalid
    {
      get
      {
        return OssResources.ResourceManager.GetString("ObjectKeyInvalid", OssResources.resourceCulture);
      }
    }

    internal OssResources()
    {
    }
  }
}
