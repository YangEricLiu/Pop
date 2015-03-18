/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Globalization;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示主键（PrimaryKey）列的值。
    /// </summary>
    /// <remarks>
    /// <para>此类型对象可以直接与<see cref="String"/>、<see cref="Int64"/>和<see cref="Boolean"/>类型
    /// 相互转换。</para>
    /// <para>可以将<see cref="String"/>、<see cref="Int64"/>或<see cref="Boolean"/>对象隐式地转换为<see cref="PrimaryKeyValue"/>类型的对象，
    /// 转换后的对象的<see cref="ValueType"/>属性为相对应的<see cref="PrimaryKeyType"/>枚举的值。
    /// 例如，将一个<see cref="Int64"/>对象转换为<see cref="PrimaryKeyValue"/>，则转换后对象的<see cref="ValueType"/>等于<see cref="PrimaryKeyType.Integer"/>。</para>
    /// <para>可以将<see cref="PrimaryKeyValue"/>显式地转换为<see cref="String"/>、<see cref="Int64"/>或<see cref="Boolean"/>。
    /// 但需要根据<see cref="ValueType"/>指定的类型进行转换，否则会抛出<see cref="InvalidCastException"/>的异常。</para>
    /// <para>调用<see cref="ToString()"/>方法可以得到值的字符串表示形式。</para>
    /// </remarks>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.ValueConversion"]/*'/>
    public struct PrimaryKeyValue
    {
        /// <summary>
        /// 获取值的字符串表示。
        /// </summary>
        internal string Value { get;  set; }

        /// <summary>
        /// 表示该主键值是不是主键范围（无限大或无限小）。
        /// </summary>
        internal bool IsInf { get;  set; }

        /// <summary>
        /// 获取值的数据类型。
        /// </summary>
        public PrimaryKeyType ValueType { get;  set; }

        /// <summary>
        /// 初始化新的<see cref="PrimaryKeyValue"/>实例。
        /// </summary>
        /// <remarks>
        /// 对用户隐藏该构造函数，以防止用户提供错误的值。
        /// 比如类型指定为INTEGER，但给定值并非合法的表示整数的字符串。
        /// </remarks>
        /// <param name="value">值的字符串表示。</param>
        /// <param name="valueType">值的数据类型。</param>
        internal PrimaryKeyValue(string value, PrimaryKeyType valueType)
            : this(value, valueType, false)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="PrimaryKeyValue"/>实例。
        /// </summary>
        /// <remarks>
        /// 对用户隐藏该构造函数，以防止用户提供错误的值。
        /// 比如类型指定为INTEGER，但给定值并非合法的表示整数的字符串。
        /// </remarks>
        /// <param name="value">值的字符串表示。</param>
        /// <param name="valueType">值的数据类型。</param>
        /// <param name="isInf">表示该主键值是不是主键范围（无限大或无限小）。</param>
        internal PrimaryKeyValue(string value, PrimaryKeyType valueType, bool isInf)
            : this()
        {
            // 主键值不能是null，但允许为空字符串。
            if (value == null)
                throw new ArgumentNullException("value");

            this.Value = value;
            this.ValueType = valueType;
            this.IsInf = isInf;
        }

        /// <summary>
        /// 比较是否与另一实例相等。
        /// </summary>
        /// <param name="obj">要比较的实例。</param>
        /// <returns>true相等，false不相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(PrimaryKeyValue))
            {
                return false;
            }

            var val = (PrimaryKeyValue)obj;

            if (this.ValueType != val.ValueType || this.IsInf != val.IsInf)
            {
                return false;
            }

            if (this.ValueType == PrimaryKeyType.String)
            {
                return this.Value == val.Value;
            }
            else
            {
                return String.Compare(this.Value, val.Value,
                                      CultureInfo.InvariantCulture,
                                      CompareOptions.IgnoreCase) == 0;
            }
        }

        /// <summary>
        /// 获取对象的哈希值。
        /// </summary>
        /// <returns>哈希值。</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode() ^ this.ValueType.GetHashCode();
        }

        #region Operators

        /// <summary>
        /// 相等操作符。
        /// </summary>
        /// <param name="leftValue">左值。</param>
        /// <param name="rightValue">右值。</param>
        /// <returns>true相等，false不相等。</returns>
        public static bool operator ==(PrimaryKeyValue leftValue, PrimaryKeyValue rightValue)
        {
            return leftValue.Equals(rightValue);
        }

        /// <summary>
        /// 不相等操作符。
        /// </summary>
        /// <param name="leftValue">左值。</param>
        /// <param name="rightValue">右值。</param>
        /// <returns>true不相等，false相等。</returns>
        public static bool operator !=(PrimaryKeyValue leftValue, PrimaryKeyValue rightValue)
        {
            return !leftValue.Equals(rightValue);
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="String"/>对象转换为<see cref="PrimaryKeyValue"/>对象。
        /// 主键值不能为空引用或者长度为0的字符串。
        /// </summary>
        /// <param name="value"><see cref="String"/>对象。</param>
        /// <returns><see cref="PrimaryKeyValue"/>对象。</returns>
        public static implicit operator PrimaryKeyValue(String value)
        {
            return new PrimaryKeyValue(value, PrimaryKeyType.String);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="PrimaryKeyValue"/>对象转换为<see cref="String"/>对象。
        /// </summary>
        /// <param name="value"><see cref="PrimaryKeyValue"/>对象。</param>
        /// <returns><see cref="String"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="PrimaryKeyValue.ValueType"/>不等于<see cref="PrimaryKeyType.String"/>。</exception>
        public static explicit operator String(PrimaryKeyValue value)
        {
            if (value.ValueType != PrimaryKeyType.String)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "String"));

            return value.Value;
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="Int64"/>对象转换为<see cref="PrimaryKeyValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="Int64"/>对象。</param>
        /// <returns><see cref="PrimaryKeyValue"/>对象。</returns>
        public static implicit operator PrimaryKeyValue(Int64 value)
        {
            return new PrimaryKeyValue(value.ToString(CultureInfo.InvariantCulture), PrimaryKeyType.Integer);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="PrimaryKeyValue"/>对象转换为<see cref="Int64"/>对象。
        /// </summary>
        /// <param name="value"><see cref="PrimaryKeyValue"/>对象。</param>
        /// <returns><see cref="Int64"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="PrimaryKeyValue.ValueType"/>不等于<see cref="PrimaryKeyType.Integer"/>。</exception>
        public static explicit operator Int64(PrimaryKeyValue value)
        {
            if (value.ValueType != PrimaryKeyType.Integer)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "Int64"));

            Int64 result;
            if (Int64.TryParse(value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, "Int64"));
            }
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="Boolean"/>对象转换为<see cref="PrimaryKeyValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="Boolean"/>对象。</param>
        /// <returns><see cref="PrimaryKeyValue"/>对象。</returns>
        public static implicit operator PrimaryKeyValue(Boolean value)
        {
            return new PrimaryKeyValue(value.ToString(CultureInfo.InvariantCulture), PrimaryKeyType.Boolean);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="PrimaryKeyValue"/>对象转换为<see cref="Boolean"/>对象。
        /// </summary>
        /// <param name="value"><see cref="PrimaryKeyValue"/>对象。</param>
        /// <returns><see cref="Boolean"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="PrimaryKeyValue.ValueType"/>不等于<see cref="PrimaryKeyType.Boolean"/>。</exception>
        public static explicit operator Boolean(PrimaryKeyValue value)
        {
            if (value.ValueType != PrimaryKeyType.Boolean)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "Boolean"));

            Boolean result;
            if (Boolean.TryParse(value.Value, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, "Boolean"));
            }
        }

        #endregion

        #region Converters

        /// <summary>
        /// 获取值的字符串表示。
        /// </summary>
        /// <returns>值的字符串。</returns>
        public override string ToString()
        {
            if (this.Value == null)
            {
                return string.Empty;
            }
            else
            {
                return this.Value;
            }
        }

        #endregion

        /// <summary>
        /// Compares the value to another one.
        /// </summary>
        /// <param name="right">The value to compare to.</param>
        /// <returns>1 : the left is greater; 0 : equal; -1 : the right is greater.</returns>
        internal int CompareTo(PrimaryKeyValue right)
        {
            PrimaryKeyValue left = this;
            if (left == PrimaryKeyRange.InfMax)
            {
                return right == PrimaryKeyRange.InfMax ? 0 : 1;
            }
            if (left == PrimaryKeyRange.InfMin)
            {
                return right == PrimaryKeyRange.InfMin ? 0 : -1;
            }
            if (right == PrimaryKeyRange.InfMin)
            {
                return left == PrimaryKeyRange.InfMin ? 0 : 1;
            }
            if (right == PrimaryKeyRange.InfMax)
            {
                return left == PrimaryKeyRange.InfMax ? 0 : -1;
            }

            Debug.Assert(!left.IsInf && !right.IsInf && left.ValueType == right.ValueType);
            if (left.ValueType == PrimaryKeyType.Integer)
            {
                return ((long)left).CompareTo((long)right);
            }

            if (left.ValueType == PrimaryKeyType.Boolean)
            {
                // OTS considers false < true.
                if (left.Value == right.Value)
                {
                    return 0;
                }
                else if ((bool)left == true)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            Debug.Assert(left.ValueType == PrimaryKeyType.String, "Unsupported PrimaryKeyType:" + left.ValueType.ToString());
            return StringComparer.Ordinal.Compare(left.Value, right.Value);
        }
    }
}
