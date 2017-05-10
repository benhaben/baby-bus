// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.ColumnValue
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService
{
  /// <summary>
  /// 表示一行中数据列的值。
  /// 
  /// </summary>
  /// 
  /// <remarks>
  /// 
  /// <para>
  /// 此类型对象可以直接与<see cref="T:System.String"/>、<see cref="T:System.Int64"/>、<see cref="T:System.Boolean"/>和<see cref="T:System.Double"/>类型
  ///             相互转换。
  /// </para>
  /// 
  /// <para>
  /// 可以将<see cref="T:System.String"/>、<see cref="T:System.Int64"/>、<see cref="T:System.Boolean"/>或<see cref="T:System.Double"/>对象隐式地转换为<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>类型的对象，
  ///             转换后的对象的<see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>属性为相对应的<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnType"/>枚举的值。
  ///             例如，将一个<see cref="T:System.Int64"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>，则转换后对象的<see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>等于<see cref="F:Aliyun.OpenServices.OpenTableService.ColumnType.Integer"/>。
  /// </para>
  /// 
  /// <para>
  /// 可以将<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>显式地转换为<see cref="T:System.String"/>、<see cref="T:System.Int64"/>、<see cref="T:System.Boolean"/>或<see cref="T:System.Double"/>。
  ///             但需要根据<see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>指定的类型进行转换，否则会抛出<see cref="T:System.InvalidCastException"/>的异常。
  /// </para>
  /// 
  /// <para>
  /// 调用<see cref="M:Aliyun.OpenServices.OpenTableService.ColumnValue.ToString"/>方法可以得到值的字符串表示形式。
  /// </para>
  /// 
  /// </remarks>
  /// 
  /// <example>
  /// 下面的示例代码获取指定员工信息并根据基础工资和奖金比率计算奖金。示例中使用了<see cref="M:Aliyun.OpenServices.OpenTableService.OtsClient.GetRow(Aliyun.OpenServices.OpenTableService.SingleRowQueryCriteria)"/>方法，并进行了类型转换。
  /// 
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.Linq;
  /// using Aliyun.OpenServices.OpenTableService;
  /// 
  /// namespace Aliyun.OpenServices.Samples.OpenTableService
  /// {
  ///     class ValueConversionSample
  ///     {
  ///         string endpoint = "http://ots.aliyuncs.com";
  ///         string accessId = "<your access id>";
  ///         string accessKey = "<your access key>";
  /// 
  ///         public void ComputeBonus(string tableName, int uid, string employee)
  ///         {
  ///             // 构造查询条件，假定数据的主键是uid和name
  ///             var criteria = new SingleRowQueryCriteria(tableName);
  ///             criteria.PrimaryKeys["uid"] = uid;
  ///             criteria.PrimaryKeys["name"] = employee;
  ///             // 指定返回列，假定rate列是比率，类型为Double；salary列为基本工资，类型为Integer
  ///             criteria.ColumnNames.Add("name");
  ///             criteria.ColumnNames.Add("rate");
  ///             criteria.ColumnNames.Add("salary");
  /// 
  ///             var otsClient = new OtsClient(endpoint, accessId, accessKey);
  /// 
  ///             try
  ///             {
  ///                 // 读取数据
  ///                 var row = otsClient.GetRow(criteria);
  ///                 if (row == null)
  ///                 {
  ///                     Console.WriteLine("未找到员工{0}的信息。", employee);
  ///                 }
  /// 
  ///                 try
  ///                 {
  ///                     // 显式转换rate列的值为double类型，如果rate的ValueType != ColumnType.Double，则会抛出异常
  ///                     double rate = (double)row.Columns["rate"];
  ///                     // 显式转换rate列的值为long类型, 如果salary列的ValueType != ColumnType.Integer，则会抛出异常
  ///                     long salary = (long)row.Columns["salary"];
  /// 
  ///                     var bonus = rate * salary;
  ///                     Console.WriteLine("奖金数：{0}", bonus);
  ///                 }
  ///                 catch (InvalidCastException)
  ///                 {
  ///                     Console.WriteLine("存储数据格式错误。");
  ///                 }
  ///             }
  ///             catch (OpenTableServiceException ex)
  ///             {
  ///                 Console.WriteLine("插入数据失败。OTS异常消息： " + ex.Message);
  ///                 Console.WriteLine("Request ID: {0}\tHostID: {1}", ex.RequestId, ex.HostId);
  ///             }
  ///             catch (System.Net.WebException ex)
  ///             {
  ///                 Console.WriteLine("创建表失败。网络异常：{0}。请检查Endpoint或网络链接。", ex.Message);
  ///             }
  ///         }
  ///     }
  /// }
  ///         ]]>
  /// </code>
  /// 
  /// </example>
  public struct ColumnValue
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
    public ColumnType ValueType { get; private set; }

    /// <summary>
    /// 初始化<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>实例。
    /// 
    /// </summary>
    /// <param name="value">值的字符串表示。</param><param name="valueType">值的数据类型。</param>
    internal ColumnValue(string value, ColumnType valueType)
    {
      this = new ColumnValue();
      if (value == null)
        throw new ArgumentNullException("value");
      this.Value = value;
      this.ValueType = valueType;
    }

    /// <summary>
    /// 隐式转换操作符，将<see cref="T:System.String"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:System.String"/>对象。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// </returns>
    public static implicit operator ColumnValue(string value)
    {
      return new ColumnValue(value, ColumnType.String);
    }

    /// <summary>
    /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象转换为<see cref="T:System.String"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。</param>
    /// <returns>
    /// <see cref="T:System.String"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.ColumnType.String"/>。</exception>
    public static explicit operator string(ColumnValue value)
    {
      if (value.ValueType == ColumnType.String)
        return value.Value;
      throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1]
      {
        (object) "String"
      }));
    }

    /// <summary>
    /// 隐式转换操作符，将<see cref="T:System.Boolean"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:System.Boolean"/>对象。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// </returns>
    public static implicit operator ColumnValue(bool value)
    {
      return new ColumnValue(value.ToString((IFormatProvider) CultureInfo.InvariantCulture), ColumnType.Boolean);
    }

    /// <summary>
    /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象转换为<see cref="T:System.Boolean"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。</param>
    /// <returns>
    /// <see cref="T:System.Boolean"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.ColumnType.Boolean"/>。</exception>
    public static explicit operator bool(ColumnValue value)
    {
      if (value.ValueType != ColumnType.Boolean)
      {
        throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1]
        {
          (object) "Boolean"
        }));
      }
      else
      {
        bool result;
        if (bool.TryParse(value.Value, out result))
          return result;
        throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, new object[1]
        {
          (object) "Boolean"
        }));
      }
    }

    /// <summary>
    /// 隐式转换操作符，将<see cref="T:System.Int64"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:System.Int64"/>对象。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// </returns>
    public static implicit operator ColumnValue(long value)
    {
      return new ColumnValue(value.ToString((IFormatProvider) CultureInfo.InvariantCulture), ColumnType.Integer);
    }

    /// <summary>
    /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象转换为<see cref="T:System.Int64"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。</param>
    /// <returns>
    /// <see cref="T:System.Int64"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.ColumnType.Integer"/>。</exception>
    public static explicit operator long(ColumnValue value)
    {
      if (value.ValueType != ColumnType.Integer)
      {
        throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1]
        {
          (object) "Integer"
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
    /// 隐式转换操作符，将<see cref="T:System.Double"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:System.Double"/>对象。</param>
    /// <returns>
    /// <see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException">value的值为<see cref="F:System.Double.NaN"/>。</exception>
    public static implicit operator ColumnValue(double value)
    {
      if (double.IsNaN(value))
        throw new InvalidCastException(OtsExceptions.CannotCastDoubleNaN);
      else
        return new ColumnValue(value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture), ColumnType.Double);
    }

    /// <summary>
    /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象转换为<see cref="T:System.Double"/>对象。
    /// 
    /// </summary>
    /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.ColumnValue"/>对象。</param>
    /// <returns>
    /// <see cref="T:System.Double"/>对象。
    /// </returns>
    /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.ColumnValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.ColumnType.Double"/>。</exception>
    public static explicit operator double(ColumnValue value)
    {
      if (value.ValueType != ColumnType.Double)
      {
        throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1]
        {
          (object) "Double"
        }));
      }
      else
      {
        try
        {
          return Convert.ToDouble(value.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        catch (OverflowException ex)
        {
          return value.Value.StartsWith("-", StringComparison.OrdinalIgnoreCase) ? double.MinValue : double.MaxValue;
        }
        catch (FormatException ex)
        {
          throw new InvalidCastException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, new object[1]
          {
            (object) "Int64"
          }), (Exception) ex);
        }
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
    public static bool operator ==(ColumnValue leftValue, ColumnValue rightValue)
    {
      return leftValue.Equals((object) rightValue);
    }

    /// <summary>
    /// 不相等操作符。
    /// 
    /// </summary>
    /// <param name="value1">左值。</param><param name="rightValue">右值。</param>
    /// <returns>
    /// 是否相符。
    /// </returns>
    public static bool operator !=(ColumnValue value1, ColumnValue rightValue)
    {
      return !value1.Equals((object) rightValue);
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
      if (obj == null || obj.GetType() != typeof (ColumnValue))
        return false;
      ColumnValue columnValue = (ColumnValue) obj;
      if (this.ValueType != columnValue.ValueType)
        return false;
      if (this.ValueType == ColumnType.String)
        return this.Value == columnValue.Value;
      else
        return string.Compare(this.Value, columnValue.Value, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0;
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
