/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示主键（Primary Key）范围。
    /// </summary>
    public class PrimaryKeyRange
    {
        private static readonly PrimaryKeyValue _infMin =
            new PrimaryKeyValue("INF_MIN", PrimaryKeyType.String, true);

        private static readonly PrimaryKeyValue _infMax =
            new PrimaryKeyValue("INF_MAX", PrimaryKeyType.String, true);

        /// <summary>
        /// 表示主键值范围的无限小值。
        /// </summary>
        public static PrimaryKeyValue InfMin
        {
            get { return _infMin; }
        }

        /// <summary>
        /// 表示主键值范围的无限大值。
        /// </summary>
        public static PrimaryKeyValue InfMax
        {
            get { return _infMax; }
        }

        /// <summary>
        /// 获取范围的主键名。
        /// </summary>
        public string PrimaryKeyName { get; private set; }

        /// <summary>
        /// 获取主键范围的开始值。
        /// </summary>
        /// <remarks>
        /// 值为查询对应的主键列的开始值（范围是一个左闭右开区间），
        /// 且类型必须与主键列指定的数据类型一致；或是<see cref="InfMin"/>。
        /// </remarks>
        public PrimaryKeyValue RangeBegin { get; private set; }

        /// <summary>
        /// 获取主键范围的结束值。
        /// </summary>
        /// <remarks>
        /// 值为查询对应的主键列的结束值（范围是一个左闭右开区间），
        /// 且类型必须与主键列指定的数据类型一致；或是<see cref="InfMax"/>。
        /// </remarks>
        public PrimaryKeyValue RangeEnd { get; private set; }

        /// <summary>
        /// 获取主键范围的数据类型。
        /// </summary>
        public PrimaryKeyType RangeType { get; private set; }

        /// <summary>
        /// 初始化新的<see cref="PrimaryKeyRange"/>实例。
        /// 表示左闭右开的主键范围。
        /// 该构造函数将使用begin和end中不为<see cref="InfMin"/>或
        /// <see cref="InfMax"/>的那一个的数据类型作为<see cref="RangeType"/>。
        /// 如果begin为<see cref="InfMin"/>的同时end为<see cref="InfMax"/>，
        /// 则<see cref="RangeType"/>默认为<see cref="PrimaryKeyType.String"/>；
        /// 如果需要指定<see cref="RangeType"/>，请使用构造函数的另一个重载：
        /// <see cref="PrimaryKeyRange(String, PrimaryKeyValue, PrimaryKeyValue, PrimaryKeyType)"/>。
        /// </summary>
        /// <remarks>
        /// rangeBegin和rangeEnd的数据类型必须与primaryKeyName指定的主键列的数据类型相匹配。
        /// 并且rangeBegin需要小于或等于rangeEnd。
        /// 整型按数字大小比较；字符型按字典顺序比较；
        /// 布尔型的false小于true；
        /// 任何类型的值均大于PrimaryKeyRange.InfMin且小于PrimaryKeyRange.InfMax。
        /// </remarks>
        /// <param name="primaryKeyName">范围的主键名。</param>
        /// <param name="rangeBegin">
        /// 查询对应的主键列的起始值，类型必须与主键列指定的数据类型一致；或是<see cref="InfMin"/>。
        /// </param>
        /// <param name="rangeEnd">
        /// 查询对应的主键列的结束值，类型必须与主键列指定的数据类型一致；或是<see cref="InfMax"/>。
        /// </param>
        public PrimaryKeyRange(String primaryKeyName, PrimaryKeyValue rangeBegin, PrimaryKeyValue rangeEnd)
            : this(primaryKeyName, rangeBegin, rangeEnd, !rangeBegin.IsInf ? rangeBegin.ValueType : !rangeEnd.IsInf ? rangeEnd.ValueType : PrimaryKeyType.String)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="PrimaryKeyRange"/>实例。
        /// 表示左闭右开的主键范围。
        /// 该构造函数主要是为了rangeBegin为<see cref="InfMin"/>的同时rangeEnd为<see cref="InfMax"/>的情形。
        /// 如果不需要，请使用另一个重载：
        /// <see cref="PrimaryKeyRange(String, PrimaryKeyValue, PrimaryKeyValue)"/>，可以推断主键的数据类型。
        /// </summary>
        /// <remarks>
        /// rangeBegin和rangeEnd的数据类型必须与primaryKeyName指定的主键列的数据类型相匹配。
        /// 并且rangeBegin需要小于或等于rangeEnd。
        /// 整型按数字大小比较；字符型按字典顺序比较；
        /// 布尔型的false小于true；
        /// 任何类型的值均大于PrimaryKeyRange.InfMin且小于PrimaryKeyRange.InfMax。
        /// </remarks>
        /// <param name="primaryKeyName">范围的主键名。</param>
        /// <param name="rangeBegin">查询对应的主键列的起始值，类型必须与主键列指定的数据类型一致；或是<see cref="InfMin"/>。</param>
        /// <param name="rangeEnd">查询对应的主键列的结束值，类型必须与主键列指定的数据类型一致；或是<see cref="InfMax"/>。</param>
        /// <param name="primaryKeyType">主键的数据类型。</param>
        public PrimaryKeyRange(String primaryKeyName, PrimaryKeyValue rangeBegin, PrimaryKeyValue rangeEnd,
                               PrimaryKeyType primaryKeyType)
        {
            if (string.IsNullOrEmpty(primaryKeyName) || !OtsUtility.IsEntityNameValid(primaryKeyName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "primaryKeyName");

            if ((!rangeBegin.IsInf && rangeBegin.ValueType != primaryKeyType ||
                 (!rangeEnd.IsInf && rangeEnd.ValueType != primaryKeyType)))
                throw new ArgumentException(OtsExceptions.RangeTypeNotMatch);

            this.PrimaryKeyName = primaryKeyName;
            this.RangeBegin = rangeBegin;
            this.RangeEnd = rangeEnd;
            this.RangeType = primaryKeyType;
        }
    }
}
