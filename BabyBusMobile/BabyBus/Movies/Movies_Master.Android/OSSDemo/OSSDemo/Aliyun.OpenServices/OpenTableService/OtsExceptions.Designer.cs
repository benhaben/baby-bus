// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.OtsExceptions
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// A strongly-typed resource class, for looking up localized strings, etc.
  /// 
  /// </summary>
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class OtsExceptions
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
        if (object.ReferenceEquals((object) OtsExceptions.resourceMan, (object) null))
          OtsExceptions.resourceMan = new ResourceManager("Aliyun.OpenServices.OpenTableService.OtsExceptions", typeof (OtsExceptions).Assembly);
        return OtsExceptions.resourceMan;
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
        return OtsExceptions.resourceCulture;
      }
      set
      {
        OtsExceptions.resourceCulture = value;
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 不能将Double.NaN转换为ColumnValue类型的对象，OTS不支持值为NaN的Double值，请避免使用。.
    /// 
    /// </summary>
    internal static string CannotCastDoubleNaN
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("CannotCastDoubleNaN", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 解码返回结果中列的值时出现错误。.
    /// 
    /// </summary>
    internal static string ColumnValueCannotBeDecoded
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ColumnValueCannotBeDecoded", OtsExceptions.resourceCulture);
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
        return OtsExceptions.ResourceManager.GetString("EndpointNotSupportedProtocal", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 指定分页键的值无效。该值必须为0，或小于表（Table）结构或视图（View）中定义的主键个数的正数。.
    /// 
    /// </summary>
    internal static string InvalidPagingKeyLength
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("InvalidPagingKeyLength", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to RequestID: {0}, HostID: {1}。.
    /// 
    /// </summary>
    internal static string InvalidResponseMessage
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("InvalidResponseMessage", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 表（Table）、表组（Table Group）、视图（View）、主键（Primary Key）或列（Column）的名称无效。OTS中名称必须由数字、英文字母或下划线组成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的总长度不能超过100个字符。.
    /// 
    /// </summary>
    internal static string NameFormatIsInvalid
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("NameFormatIsInvalid", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 需要至少给定一个主键（Primary Key）。.
    /// 
    /// </summary>
    internal static string NoPrimaryKeySpecified
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("NoPrimaryKeySpecified", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to rowChanges中至少需要包含一行数据。.
    /// 
    /// </summary>
    internal static string NoRowForBatchModifyData
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("NoRowForBatchModifyData", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 请求失败，原因： {0}；OTS错误代码: {1}。.
    /// 
    /// </summary>
    internal static string OpenTableServiceExceptionMessageFormat
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("OpenTableServiceExceptionMessageFormat", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 请求的返回结果验证失败，原因：{0}。.
    /// 
    /// </summary>
    internal static string OpenTableServiceInvalidResponseMessageFormat
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("OpenTableServiceInvalidResponseMessageFormat", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 当正向读取时，PrimaryKeyRange.InfMin和PrimaryKeyRange.InfMax只能依次应用于PrimaryKeyRange.RangeBegin和PrimaryKeyRange.RangeEnd。当反向读取时，PrimaryKeyRange.InfMin和PrimaryKeyRange.InfMax只能依次应用于PrimaryKeyRange.RangeEnd和PrimaryKeyRange.RangeBegin。.
    /// 
    /// </summary>
    internal static string PKInfNotAllowed
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("PKInfNotAllowed", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 主键（Primary Key）的值无效，不能为空引用或值为长度是零的字符串。.
    /// 
    /// </summary>
    internal static string PrimaryKeyValueIsNullOrEmpty
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("PrimaryKeyValueIsNullOrEmpty", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 必须显式地给定Top属性的值，且必须大于或等于0。.
    /// 
    /// </summary>
    internal static string QueryTopValueNotSet
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("QueryTopValueNotSet", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 值必须大于或等于零。.
    /// 
    /// </summary>
    internal static string QueryTopValueOutOfRange
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("QueryTopValueOutOfRange", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 当正向读取时，主键范围的起始值需要小于或等于结束值。当反向读取时，主键范围的起始值需要大于或等于结束值。整型按数字大小比较；字符型按字典顺序比较；布尔型的false小于true；任何类型的值均大于PrimaryKeyRange.InfMin且小于PrimaryKeyRange.InfMax。.
    /// 
    /// </summary>
    internal static string RangeBeginGreaterThanRangeEnd
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("RangeBeginGreaterThanRangeEnd", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 主键范围的没有设置，请先设置Range属性。.
    /// 
    /// </summary>
    internal static string RangeNotSet
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("RangeNotSet", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to rangeBegin和rangeEnd的数据类型必须与给定主键的数据类型相匹配。.
    /// 
    /// </summary>
    internal static string RangeTypeNotMatch
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("RangeTypeNotMatch", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 返回结果数据不完整。.
    /// 
    /// </summary>
    internal static string ResponseDataIncomplete
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ResponseDataIncomplete", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 返回结果缺失头“{0}”。.
    /// 
    /// </summary>
    internal static string ResponseDoesNotContainHeader
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ResponseDoesNotContainHeader", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 返回结果超时。.
    /// 
    /// </summary>
    internal static string ResponseExpired
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ResponseExpired", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 返回验证失败。.
    /// 
    /// </summary>
    internal static string ResponseFailedAuthorization
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ResponseFailedAuthorization", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 此版本的SDK只支持Base64格式的数据编码。请考虑升级SDK以支持编码{0}。.
    /// 
    /// </summary>
    internal static string UnsupportedEncodingFormat
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("UnsupportedEncodingFormat", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 解析失败，无法将值转换转换为{0}类型。.
    /// 
    /// </summary>
    internal static string ValueCastFailedFormat
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ValueCastFailedFormat", OtsExceptions.resourceCulture);
      }
    }

    /// <summary>
    /// Looks up a localized string similar to 值的数据类型不是{0}。.
    /// 
    /// </summary>
    internal static string ValueCastInvalidTypeFormat
    {
      get
      {
        return OtsExceptions.ResourceManager.GetString("ValueCastInvalidTypeFormat", OtsExceptions.resourceCulture);
      }
    }

    internal OtsExceptions()
    {
    }
  }
}
