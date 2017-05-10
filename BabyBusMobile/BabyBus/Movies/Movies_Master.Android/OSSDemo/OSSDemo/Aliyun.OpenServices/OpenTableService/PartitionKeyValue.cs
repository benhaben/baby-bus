// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.PartitionKeyValue
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示分片键的值。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// 此类型对象可以直接与<see cref="T:System.String"/>或<see cref="T:System.Int64"/>类型相互转换。
  /// </para>
  /// 
  /// <para>
  /// 可以将<see cref="T:System.String"/>或<see cref="T:System.Int64"/>对象隐式地转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>类型的对象，
  ///             转换后的对象的<see cref="P:Aliyun.OpenServices.OpenTableService.PartitionKeyValue.ValueType"/>属性为相对应的<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyType"/>枚举的值。
  ///             例如，将一个<see cref="T:System.Int64"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>，则转换后对象的<see cref="P:Aliyun.OpenServices.OpenTableService.PartitionKeyValue.ValueType"/>等于<see cref="F:Aliyun.OpenServices.OpenTableService.PartitionKeyType.Integer"/>。
  /// </para>
  /// 
  /// <para>
  /// 可以将<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>显式地转换为<see cref="T:System.String"/>或<see cref="T:System.Int64"/>。
  ///             但需要根据<see cref="P:Aliyun.OpenServices.OpenTableService.PartitionKeyValue.ValueType"/>指定的类型进行转换，否则会抛出<see cref="T:System.InvalidCastException"/>的异常。
  /// </para>
  /// 
  /// <para>
  /// 调用<see cref="M:Aliyun.OpenServices.OpenTableService.PartitionKeyValue.ToString"/>方法可以得到值的字符串表示形式。
  /// </para>
  /// 
  /// </remarks>
  public struct PartitionKeyValue
  {
    /// <summary>
    /// 获取值的字符串表示。
    /// 
    /// </summary>
    internal string Value { get; private set; }

    /// <summary>
    /// 获取值的数据类型。
    /// 
    /// </summary>
    public PartitionKeyType ValueType { get; private set; }

    /// <summary>
    /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>实例。
    /// 
    /// </summary>
    /// <param name="value">值的字符串表示。</param><param name="valueType">值的数据类型。</param>
    internal PartitionKeyValue(string value, PartitionKeyType valueType)
    {
      this = new PartitionKeyValue();
      if (value == null)
        throw new ArgumentNullException("value");
      this.Value = value;
      this.ValueType = valueType;
    }

    /// <summary>
    /// 隐式转换操作符，将<see cref="T:System.String"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:System.String"/>对象。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象。
    /// </returns>
    public static implicit operator PartitionKeyValue(string value)
    {
      return new PartitionKeyValue(value, PartitionKeyType.String);
    }

    /// <summary>
    /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象转换为<see cref="T:System.String"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象。</param>
    /// <returns>
    /// <see cref="T:System.String"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.PartitionKeyValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.PartitionKeyType.String"/>。</exception>
    public static explicit operator string(PartitionKeyValue value)
    {
      if (value.ValueType == PartitionKeyType.String)
        return value.Value;
      throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1]
      {
        (object) "String"
      }));
    }

    /// <summary>
    /// 隐式转换操作符，将<see cref="T:System.Int64"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:System.Int64"/>对象。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象。
    /// </returns>
    public static implicit operator PartitionKeyValue(long value)
    {
      return new PartitionKeyValue(value.ToString((IFormatProvider) CultureInfo.InvariantCulture), PartitionKeyType.Integer);
    }

    /// <summary>
    /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象转换为<see cref="T:System.Int64"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.PartitionKeyValue"/>对象。</param>
    /// <returns>
    /// <see cref="T:System.Int64"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.PartitionKeyValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.PartitionKeyType.Integer"/>。</exception>
    public static explicit operator long(PartitionKeyValue value)
    {
      if (value.ValueType != PartitionKeyType.Integer)
      {
        throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1]
        {
          (object) "Int64"
        }));
      }
      else
      {
        long result;
        if (long.TryParse(value.Value, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return result;
        throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, new object[1]
        {
          (object) "Int64"
        }));
      }
    }

    /// <summary>
    /// 相等操作符。
    /// 
    /// </summary>
    /// <param name="leftValue">左值。</param><param name="rightValue">右值。</param>
    /// <returns>
    /// true相等，false不相等。
    /// </returns>
    public static bool operator ==(PartitionKeyValue leftValue, PartitionKeyValue rightValue)
    {
      return leftValue.Equals((object) rightValue);
    }

    /// <summary>
    /// 不相等操作符。
    /// 
    /// </summary>
    /// <param name="leftValue">左值。</param><param name="rightValue">右值。</param>
    /// <returns>
    /// true不相等，false相等。
    /// </returns>
    public static bool operator !=(PartitionKeyValue leftValue, PartitionKeyValue rightValue)
    {
      return !leftValue.Equals((object) rightValue);
    }

    /// <summary>
    /// 比较是否与另一实例相等。
    /// 
    /// </summary>
    /// <param name="obj">要比较的实例。</param>
    /// <returns>
    /// true相等，false不相等。
    /// </returns>
    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != typeof (PartitionKeyValue))
        return false;
      PartitionKeyValue partitionKeyValue = (PartitionKeyValue) obj;
      return this.ValueType == partitionKeyValue.ValueType && this.Value == partitionKeyValue.Value;
    }

    /// <summary>
    /// 获取对象的哈希值。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 哈希值。
    /// </returns>
    public override int GetHashCode()
    {
      return this.Value.GetHashCode() ^ this.ValueType.GetHashCode();
    }

    /// <summary>
    /// 获取值的字符串表示。
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// 值的字符串。
    /// </returns>
    public override string ToString()
    {
      if (this.Value == null)
        return string.Empty;
      else
        return this.Value;
    }
  }
}
