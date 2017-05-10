// Decompiled with JetBrains decompiler
// Type: Aliyun.OpenServices.OpenTableService.PrimaryKeyValue
// Assembly: Aliyun.OpenServices, Version=1.0.5290.21916, Culture=neutral, PublicKeyToken=0ad4175f0dac0b9b
// MVID: 6C2A4E91-5F65-4F0D-B29C-34B3D99D5AA0
// Assembly location: Y:\Downloads\aliyun_dotnet_sdk_20140626 (1)\bin\Aliyun.OpenServices.dll

using System;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示主键（PrimaryKey）列的值。
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// 此类型对象可以直接与<see cref="T:System.String"/>、<see cref="T:System.Int64"/>和<see cref="T:System.Boolean"/>类型
    ///             相互转换。
    /// </para>
    /// 
    /// <para>
    /// 可以将<see cref="T:System.String"/>、<see cref="T:System.Int64"/>或<see cref="T:System.Boolean"/>对象隐式地转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>类型的对象，
    ///             转换后的对象的<see cref="P:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ValueType"/>属性为相对应的<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyType"/>枚举的值。
    ///             例如，将一个<see cref="T:System.Int64"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>，则转换后对象的<see cref="P:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ValueType"/>等于<see cref="F:Aliyun.OpenServices.OpenTableService.PrimaryKeyType.Integer"/>。
    /// </para>
    /// 
    /// <para>
    /// 可以将<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>显式地转换为<see cref="T:System.String"/>、<see cref="T:System.Int64"/>或<see cref="T:System.Boolean"/>。
    ///             但需要根据<see cref="P:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ValueType"/>指定的类型进行转换，否则会抛出<see cref="T:System.InvalidCastException"/>的异常。
    /// </para>
    /// 
    /// <para>
    /// 调用<see cref="M:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ToString"/>方法可以得到值的字符串表示形式。
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
    public struct PrimaryKeyValue
    {
        /// <summary>
        /// 获取值的字符串表示。
        /// 
        /// </summary>
        internal string Value { get; private set; }

        /// <summary>
        /// 表示该主键值是不是主键范围（无限大或无限小）。
        /// 
        /// </summary>
        internal bool IsInf { get; private set; }

        /// <summary>
        /// 获取值的数据类型。
        /// 
        /// </summary>
        public PrimaryKeyType ValueType { get; private set; }

        /// <summary>
        /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>实例。
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// 对用户隐藏该构造函数，以防止用户提供错误的值。
        ///             比如类型指定为INTEGER，但给定值并非合法的表示整数的字符串。
        /// 
        /// </remarks>
        /// <param name="value">值的字符串表示。</param><param name="valueType">值的数据类型。</param>
        internal PrimaryKeyValue (string value, PrimaryKeyType valueType)
        {
            this = new PrimaryKeyValue (value, valueType, false);
        }

        /// <summary>
        /// 初始化新的<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>实例。
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// 对用户隐藏该构造函数，以防止用户提供错误的值。
        ///             比如类型指定为INTEGER，但给定值并非合法的表示整数的字符串。
        /// 
        /// </remarks>
        /// <param name="value">值的字符串表示。</param><param name="valueType">值的数据类型。</param><param name="isInf">表示该主键值是不是主键范围（无限大或无限小）。</param>
        internal PrimaryKeyValue (string value, PrimaryKeyType valueType, bool isInf)
        {
            this = new PrimaryKeyValue ();
            if (value == null)
                throw new ArgumentNullException ("value");
            this.Value = value;
            this.ValueType = valueType;
            this.IsInf = isInf;
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="T:System.String"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。
        ///             主键值不能为空引用或者长度为0的字符串。
        /// 
        /// </summary>
        /// <param name="value"><see cref="T:System.String"/>对象。</param>
        /// <returns>
        /// <see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。
        /// </returns>
        public static implicit operator PrimaryKeyValue (string value)
        {
            return new PrimaryKeyValue (value, PrimaryKeyType.String);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象转换为<see cref="T:System.String"/>对象。
        /// 
        /// </summary>
        /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。</param>
        /// <returns>
        /// <see cref="T:System.String"/>对象。
        /// </returns>
        /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.PrimaryKeyType.String"/>。</exception>
        public static explicit operator string (PrimaryKeyValue value)
        {
            if (value.ValueType == PrimaryKeyType.String)
                return value.Value;
            throw new InvalidCastException (string.Format ((IFormatProvider)CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1] {
                (object)"String"
            }));
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="T:System.Int64"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。
        /// 
        /// </summary>
        /// <param name="value"><see cref="T:System.Int64"/>对象。</param>
        /// <returns>
        /// <see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。
        /// </returns>
        public static implicit operator PrimaryKeyValue (long value)
        {
            return new PrimaryKeyValue (value.ToString ((IFormatProvider)CultureInfo.InvariantCulture), PrimaryKeyType.Integer);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象转换为<see cref="T:System.Int64"/>对象。
        /// 
        /// </summary>
        /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。</param>
        /// <returns>
        /// <see cref="T:System.Int64"/>对象。
        /// </returns>
        /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.PrimaryKeyType.Integer"/>。</exception>
        public static explicit operator long (PrimaryKeyValue value)
        {
            if (value.ValueType != PrimaryKeyType.Integer) {
                throw new InvalidCastException (string.Format ((IFormatProvider)CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1] {
                    (object)"Int64"
                }));
            } else {
                long result;
                if (long.TryParse (value.Value, NumberStyles.Integer, (IFormatProvider)CultureInfo.InvariantCulture, out result))
                    return result;
                throw new InvalidCastException (string.Format ((IFormatProvider)CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, new object[1] {
                    (object)"Int64"
                }));
            }
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="T:System.Boolean"/>对象转换为<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。
        /// 
        /// </summary>
        /// <param name="value"><see cref="T:System.Boolean"/>对象。</param>
        /// <returns>
        /// <see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。
        /// </returns>
        public static implicit operator PrimaryKeyValue (bool value)
        {
            return new PrimaryKeyValue (value.ToString ((IFormatProvider)CultureInfo.InvariantCulture), PrimaryKeyType.Boolean);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象转换为<see cref="T:System.Boolean"/>对象。
        /// 
        /// </summary>
        /// <param name="value"><see cref="T:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue"/>对象。</param>
        /// <returns>
        /// <see cref="T:System.Boolean"/>对象。
        /// </returns>
        /// <exception cref="T:System.InvalidCastException"><see cref="P:Aliyun.OpenServices.OpenTableService.PrimaryKeyValue.ValueType"/>不等于<see cref="F:Aliyun.OpenServices.OpenTableService.PrimaryKeyType.Boolean"/>。</exception>
        public static explicit operator bool (PrimaryKeyValue value)
        {
            if (value.ValueType != PrimaryKeyType.Boolean) {
                throw new InvalidCastException (string.Format ((IFormatProvider)CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, new object[1] {
                    (object)"Boolean"
                }));
            } else {
                bool result;
                if (bool.TryParse (value.Value, out result))
                    return result;
                throw new InvalidCastException (string.Format ((IFormatProvider)CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, new object[1] {
                    (object)"Boolean"
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
        public static bool operator == (PrimaryKeyValue leftValue, PrimaryKeyValue rightValue)
        {
            return leftValue.Equals ((object)rightValue);
        }

        /// <summary>
        /// 不相等操作符。
        /// 
        /// </summary>
        /// <param name="leftValue">左值。</param><param name="rightValue">右值。</param>
        /// <returns>
        /// true不相等，false相等。
        /// </returns>
        public static bool operator != (PrimaryKeyValue leftValue, PrimaryKeyValue rightValue)
        {
            return !leftValue.Equals ((object)rightValue);
        }

        /// <summary>
        /// 比较是否与另一实例相等。
        /// 
        /// </summary>
        /// <param name="obj">要比较的实例。</param>
        /// <returns>
        /// true相等，false不相等。
        /// </returns>
        public override bool Equals (object obj)
        {
            if (obj == null || obj.GetType () != typeof(PrimaryKeyValue))
                return false;
            PrimaryKeyValue primaryKeyValue = (PrimaryKeyValue)obj;
            if (this.ValueType != primaryKeyValue.ValueType || this.IsInf != primaryKeyValue.IsInf)
                return false;
            if (this.ValueType == PrimaryKeyType.String)
                return this.Value == primaryKeyValue.Value;
            else
                return string.Compare (this.Value, primaryKeyValue.Value, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0;
        }

        /// <summary>
        /// 获取对象的哈希值。
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// 哈希值。
        /// </returns>
        public override int GetHashCode ()
        {
            return this.Value.GetHashCode () ^ this.ValueType.GetHashCode ();
        }

        /// <summary>
        /// 获取值的字符串表示。
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// 值的字符串。
        /// </returns>
        public override string ToString ()
        {
            if (this.Value == null)
                return string.Empty;
            else
                return this.Value;
        }

        /// <summary>
        /// Compares the value to another one.
        /// 
        /// </summary>
        /// <param name="right">The value to compare to.</param>
        /// <returns>
        /// 1 : the left is greater; 0 : equal; -1 : the right is greater.
        /// </returns>
        internal int CompareTo (PrimaryKeyValue right)
        {
            PrimaryKeyValue primaryKeyValue = this;
            if (primaryKeyValue == PrimaryKeyRange.InfMax)
                return right == PrimaryKeyRange.InfMax ? 0 : 1;
            if (primaryKeyValue == PrimaryKeyRange.InfMin)
                return right == PrimaryKeyRange.InfMin ? 0 : -1;
            if (right == PrimaryKeyRange.InfMin)
                return primaryKeyValue == PrimaryKeyRange.InfMin ? 0 : 1;
            if (right == PrimaryKeyRange.InfMax)
                return primaryKeyValue == PrimaryKeyRange.InfMax ? 0 : -1;
            Debug.Assert (!primaryKeyValue.IsInf && !right.IsInf && primaryKeyValue.ValueType == right.ValueType);
            if (primaryKeyValue.ValueType == PrimaryKeyType.Integer)
                return (int)((long)primaryKeyValue.CompareTo ((long)right));
            if (primaryKeyValue.ValueType == PrimaryKeyType.Boolean) {
                if (primaryKeyValue.Value == right.Value)
                    return 0;
                return (bool)primaryKeyValue ? 1 : -1;
            } else {
                Debug.Assert (primaryKeyValue.ValueType == PrimaryKeyType.String, "Unsupported PrimaryKeyType:" + ((object)primaryKeyValue.ValueType).ToString ());
                return StringComparer.Ordinal.Compare (primaryKeyValue.Value, right.Value);
            }
        }
    }
}
