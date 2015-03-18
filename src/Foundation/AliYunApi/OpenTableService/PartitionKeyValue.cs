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
    /// 表示分片键的值。
    /// </summary>
    /// <remarks>
    /// <para>此类型对象可以直接与<see cref="String"/>或<see cref="Int64"/>类型相互转换。</para>
    /// <para>可以将<see cref="String"/>或<see cref="Int64"/>对象隐式地转换为<see cref="PartitionKeyValue"/>类型的对象，
    /// 转换后的对象的<see cref="ValueType"/>属性为相对应的<see cref="PartitionKeyType"/>枚举的值。
    /// 例如，将一个<see cref="Int64"/>对象转换为<see cref="PartitionKeyValue"/>，则转换后对象的<see cref="ValueType"/>等于<see cref="PartitionKeyType.Integer"/>。</para>
    /// <para>可以将<see cref="PartitionKeyValue"/>显式地转换为<see cref="String"/>或<see cref="Int64"/>。
    /// 但需要根据<see cref="ValueType"/>指定的类型进行转换，否则会抛出<see cref="InvalidCastException"/>的异常。</para>
    /// <para>调用<see cref="ToString()"/>方法可以得到值的字符串表示形式。</para>
    /// </remarks>
    public struct PartitionKeyValue
    {
        /// <summary>
        /// 获取值的字符串表示。
        /// </summary>
        internal string Value { get; set; }

        /// <summary>
        /// 获取值的数据类型。
        /// </summary>
        public PartitionKeyType ValueType { get; set; }

        /// <summary>
        /// 初始化新的<see cref="PartitionKeyValue"/>实例。
        /// </summary>
        /// <param name="value">值的字符串表示。</param>
        /// <param name="valueType">值的数据类型。</param>
        internal PartitionKeyValue(string value, PartitionKeyType valueType)
            : this()
        {
            // 主键值不能是null，但允许为空字符串。
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
            if (obj == null || obj.GetType() != typeof(PartitionKeyValue))
            {
                return false;
            }

            var val = (PartitionKeyValue)obj;
            return this.ValueType == val.ValueType && this.Value == val.Value;
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
        public static bool operator ==(PartitionKeyValue leftValue, PartitionKeyValue rightValue)
        {
            return leftValue.Equals(rightValue);
        }

        /// <summary>
        /// 不相等操作符。
        /// </summary>
        /// <param name="leftValue">左值。</param>
        /// <param name="rightValue">右值。</param>
        /// <returns>true不相等，false相等。</returns>
        public static bool operator !=(PartitionKeyValue leftValue, PartitionKeyValue rightValue)
        {
            return !leftValue.Equals(rightValue);
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="String"/>对象转换为<see cref="PartitionKeyValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="String"/>对象。</param>
        /// <returns><see cref="PartitionKeyValue"/>对象。</returns>
        public static implicit operator PartitionKeyValue(String value)
        {
            return new PartitionKeyValue(value, PartitionKeyType.String);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="PartitionKeyValue"/>对象转换为<see cref="String"/>对象。
        /// </summary>
        /// <param name="value"><see cref="PartitionKeyValue"/>对象。</param>
        /// <returns><see cref="String"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="PartitionKeyValue.ValueType"/>不等于<see cref="PartitionKeyType.String"/>。</exception>
        public static explicit operator String(PartitionKeyValue value)
        {
            if (value.ValueType != PartitionKeyType.String)
                throw new InvalidCastException(
                    String.Format(CultureInfo.CurrentUICulture, OtsExceptions.ValueCastInvalidTypeFormat, "String"));

            return value.Value;
        }

        /// <summary>
        /// 隐式转换操作符，将<see cref="Int64"/>对象转换为<see cref="PartitionKeyValue"/>对象。
        /// </summary>
        /// <param name="value"><see cref="Int64"/>对象。</param>
        /// <returns><see cref="PartitionKeyValue"/>对象。</returns>
        public static implicit operator PartitionKeyValue(Int64 value)
        {
            return new PartitionKeyValue(value.ToString(CultureInfo.InvariantCulture), PartitionKeyType.Integer);
        }

        /// <summary>
        /// 显式转换操作符，将<see cref="PartitionKeyValue"/>对象转换为<see cref="Int64"/>对象。
        /// </summary>
        /// <param name="value"><see cref="PartitionKeyValue"/>对象。</param>
        /// <returns><see cref="Int64"/>对象。</returns>
        /// <exception cref="InvalidCastException"><see cref="PartitionKeyValue.ValueType"/>不等于<see cref="PartitionKeyType.Integer"/>。</exception>
        public static explicit operator Int64(PartitionKeyValue value)
        {
            if (value.ValueType != PartitionKeyType.Integer)
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

        #endregion
    }
}
