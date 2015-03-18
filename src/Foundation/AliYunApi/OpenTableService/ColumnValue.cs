/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示一行中数据列的值。
    /// </summary>
    /// <remarks>
    /// <para>此类型对象可以直接与<see cref="String"/>、<see cref="Int64"/>、<see cref="Boolean"/>和<see cref="Double"/>类型
    /// 相互转换。</para>
    /// <para>可以将<see cref="String"/>、<see cref="Int64"/>、<see cref="Boolean"/>或<see cref="Double"/>对象隐式地转换为<see cref="ColumnValue"/>类型的对象，
    /// 转换后的对象的<see cref="ValueType"/>属性为相对应的<see cref="ColumnType"/>枚举的值。
    /// 例如，将一个<see cref="Int64"/>对象转换为<see cref="ColumnValue"/>，则转换后对象的<see cref="ValueType"/>等于<see cref="ColumnType.Integer"/>。</para>
    /// <para>可以将<see cref="ColumnValue"/>显式地转换为<see cref="String"/>、<see cref="Int64"/>、<see cref="Boolean"/>或<see cref="Double"/>。
    /// 但需要根据<see cref="ValueType"/>指定的类型进行转换，否则会抛出<see cref="InvalidCastException"/>的异常。</para>
    /// <para>调用<see cref="ToString()"/>方法可以得到值的字符串表示形式。</para>
    /// </remarks>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.ValueConversion"]/*'/>
    public struct ColumnValue
    {
        /// <summary>
        /// 获取值的字符串表示。
        /// </summary>
        internal string Value { get;  set; }

        /// <summary>
        /// 获取值的数据类型。
        /// </summary>
        public ColumnType ValueType { get;  set; }

        /// <summary>
        /// 初始化<see cref="ColumnValue"/>实例。
        /// </summary>
        /// <param name="value">值的字符串表示。</param>
        /// <param name="valueType">值的数据类型。</param>
        internal ColumnValue(string value, ColumnType valueType)
            : this()
        {
            // Column value could be an empty string.
            if (value == null)
                throw new ArgumentNullException("value");

            this.Value = value;
            this.ValueType = valueType;
        }

        /// <summary>
        /// 比较是否与另一实例相等。
        /// </summary>
        /// <param name="obj">要比较的实例。</param>
        /// <returns>true相等，false不相等。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ColumnValue))
            {
                return false;
            }

            var val = (ColumnValue)obj;
            if (this.ValueType != val.ValueType)
            {
                return false;
            }

            if (this.ValueType == ColumnType.String)
            {
                return this.Value == val.Value;
            }
            else
            {
                return String.Compare(this.Value, val.Value, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0;
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

        #region Operators

        /// <summary>
        /// 相等操作符。
        /// </summary>
        /// <param name="leftValue">左值。</param>
        /// <param name="rightValue">右值。</param>
        /// <returns>true相等，false不相等。</returns>
        public static bool operator ==(ColumnValue leftValue, ColumnValue rightValue)
        {
            return leftValue.Equals(rightValue);
        }

        /// <summary>
        /// 不相等操作符。
        /// </summary>
        /// <param name="value1">左值。</param>
        /// <param name="rightValue">右值。</param>
        /// <returns>是否相符。</returns>
        public static bool operator !=(ColumnValue value1, ColumnValue rightValue)
        {
            return !value1.Equals(rightValue);
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="String"/>对象转换为<see cref="ColumnValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="String"/>对象。</param>
        /// <returns><see cref="ColumnValue"/>对象。</returns>
        public static implicit operator ColumnValue(String value)
        {
            return new ColumnValue(value, ColumnType.String);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="ColumnValue"/>对象转换为<see cref="String"/>对象。
        /// </summary>
        /// <param name="value"><see cref="ColumnValue"/>对象。</param>
        /// <returns><see cref="String"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="ColumnValue.ValueType"/>不等于<see cref="ColumnType.String"/>。</exception>
        public static explicit operator String(ColumnValue value)
        {
            if (value.ValueType != ColumnType.String)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "String"));

            return value.Value;
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="Boolean"/>对象转换为<see cref="ColumnValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="Boolean"/>对象。</param>
        /// <returns><see cref="ColumnValue"/>对象。</returns>
        public static implicit operator ColumnValue(Boolean value)
        {
            return new ColumnValue(value.ToString(CultureInfo.InvariantCulture), ColumnType.Boolean);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="ColumnValue"/>对象转换为<see cref="Boolean"/>对象。
        /// </summary>
        /// <param name="value"><see cref="ColumnValue"/>对象。</param>
        /// <returns><see cref="Boolean"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="ColumnValue.ValueType"/>不等于<see cref="ColumnType.Boolean"/>。</exception>
        public static explicit operator Boolean(ColumnValue value)
        {
            if (value.ValueType != ColumnType.Boolean)
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

        /// <summary>
        /// 隐式转换操作符，将<see cref="Int64"/>对象转换为<see cref="ColumnValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="Int64"/>对象。</param>
        /// <returns><see cref="ColumnValue"/>对象。</returns>
        public static implicit operator ColumnValue(Int64 value)
        {
            return new ColumnValue(value.ToString(CultureInfo.InvariantCulture), ColumnType.Integer);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="ColumnValue"/>对象转换为<see cref="Int64"/>对象。
        /// </summary>
        /// <param name="value"><see cref="ColumnValue"/>对象。</param>
        /// <returns><see cref="Int64"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="ColumnValue.ValueType"/>不等于<see cref="ColumnType.Integer"/>。</exception>
        public static explicit operator Int64(ColumnValue value)
        {
            if (value.ValueType != ColumnType.Integer)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "Integer"));

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
        /// 隐式转换操作符，将<see cref="Double"/>对象转换为<see cref="ColumnValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="Double"/>对象。</param>
        /// <returns><see cref="ColumnValue"/>对象。</returns>
        /// <exception cref="InvalidCastException">value的值为<see cref="Double.NaN"/>。</exception>
        public static implicit operator ColumnValue(Double value)
        {
            if (Double.IsNaN(value))
            {
                throw new InvalidCastException(OtsExceptions.CannotCastDoubleNaN);
            }
            return new ColumnValue(value.ToString("R", CultureInfo.InvariantCulture), ColumnType.Double);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="ColumnValue"/>对象转换为<see cref="Double"/>对象。
        /// </summary>
        /// <param name="value"><see cref="ColumnValue"/>对象。</param>
        /// <returns><see cref="Double"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="ColumnValue.ValueType"/>不等于<see cref="ColumnType.Double"/>。</exception>
        public static explicit operator Double(ColumnValue value)
        {
            if (value.ValueType != ColumnType.Double)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "Double"));

            try
            {
                return Convert.ToDouble(value.Value, CultureInfo.InvariantCulture);
            }
            catch (OverflowException)
            {
                // To handle the precision difference between OTS server and C#.
                if (value.Value.StartsWith("-", StringComparison.OrdinalIgnoreCase))
                {
                    // less than double.MinValue
                    return Double.MinValue;
                }
                else
                {
                    return Double.MaxValue;
                }
            }
            catch (FormatException e)
            {
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastFailedFormat, "Int64"),
                    e);
            }
        }

        #endregion
    }
}
