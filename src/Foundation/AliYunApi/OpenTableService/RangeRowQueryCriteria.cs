/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示获取表（Table）或视图（View）中主键（Primary Key）的特定范围内多行数据的查询条件。
    /// </summary>
    public class RangeRowQueryCriteria : RowQueryCriteria
    {
        private PrimaryKeyDictionary _primaryKeyValues = new PrimaryKeyDictionary();
        private int _top = -1; // -1 means user does not specify this value.
        private PrimaryKeyRange _range;

        /// <summary>
        /// 获取主键（Primary Key）列名称与值的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示主键列的值<see cref="PrimaryKeyValue"/>。</para>
        /// </remarks>
        public IDictionary<string, PrimaryKeyValue> PrimaryKeys
        {
            get
            {
                return _primaryKeyValues;
            }
        }

        /// <summary>
        /// 获取<see cref="PrimaryKeyRange"/>对象。
        /// </summary>
        public PrimaryKeyRange Range
        {
            get
            {
                return _range;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _range = value;
            }
        }

        /// <summary>
        /// 获取或设置查询结果返回的行数。
        /// 设置为-1（默认值）表示返回所有结果。
        /// </summary>
        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值表示查询时是否反向读取，即是否从大到小进行读取。
        /// </summary>
        /// <remarks>
        /// 当值为False时，<see cref="PrimaryKeyRange.RangeBegin" />必须小于或等于
        /// <see cref="PrimaryKeyRange.RangeEnd" />。
        /// 当值为True时，<see cref="PrimaryKeyRange.RangeBegin" />必须大于或等于
        /// <see cref="PrimaryKeyRange.RangeEnd" />。
        /// 默认值为False。
        /// </remarks>
        public bool IsReverse { get; set; }

        /// <summary>
        /// 初始化新的<see cref="RangeRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// </exception>
        public RangeRowQueryCriteria(string tableName)
            : this(tableName, null)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="RangeRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <param name="viewName">要查询的视图（View）名。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// <para>- 或 -</para>
        /// <para>viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。</para>
        /// </exception>
        public RangeRowQueryCriteria(string tableName, string viewName)
            : this(tableName, viewName, null, null)
        {
        }

        /// <summary>
        /// 初始新的<see cref="RangeRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <param name="primaryKeys">主键（Primary Key）名称与值的对应字典。</param>
        /// <param name="columnNames">需要返回列（Column）的名称的列表。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// </exception>
        public RangeRowQueryCriteria(string tableName,
            IDictionary<string, PrimaryKeyValue> primaryKeys, IEnumerable<string> columnNames)
            : this(tableName, null, primaryKeys, columnNames)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="RangeRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param>
        /// <param name="viewName">要查询的视图（View）名。</param>
        /// <param name="primaryKeys">主键（Primary Key）名称与值的对应字典。</param>
        /// <param name="columnNames">需要返回列（Column）的名称的列表。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// <para>- 或 -</para>
        /// <para>viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。</para>
        /// </exception>
        public RangeRowQueryCriteria(string tableName, string viewName,
            IDictionary<string, PrimaryKeyValue> primaryKeys, IEnumerable<string> columnNames)
            : base(tableName, viewName, columnNames)
        {
            if (primaryKeys != null)
            {
                foreach (var pk in primaryKeys)
                {
                    this.PrimaryKeys.Add(pk.Key, pk.Value);
                }
            }
        }
    }
}
