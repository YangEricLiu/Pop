﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示从指定偏移量获取多行数据的查询条件。
    /// </summary>
    public class OffsetRowQueryCriteria : RowQueryCriteria
    {
        private int _offset;
        private int _top = -1;
        private PrimaryKeyDictionary _pagingKeys = new PrimaryKeyDictionary();

        /// <summary>
        /// 获取分页键（Paging Key）名称与值的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示列的值<see cref="PrimaryKeyValue"/>。</para>
        /// <para>分页键即建立表（Table）或视图（View）时指定的建立分页的主键（Primary Key）。</para>
        /// <para>查询时必须至少一个指定一个分布键，必须写入完整的表或视图的分页键的名称，并保证与建立时主键顺序一致。</para>
        /// </remarks>
        public IDictionary<string, PrimaryKeyValue> PagingKeys
        {
            get
            {
                return _pagingKeys;
            }
        }

        /// <summary>
        /// 获取或设置分页键的查询偏移量。
        /// </summary>
        /// <remarks>
        /// 该属性的值必须不小于0。
        /// </remarks>
        public int Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", OtsExceptions.QueryTopValueOutOfRange);

                _offset = value;
            }
        }

        internal bool isTopSet
        {
            get { return _top >= 0; }
        }

        /// <summary>
        /// 获取或设置查询结果返回的行数。
        /// </summary>
        /// <remarks>
        /// 该属性的值是必须显式给定的，且必须不小于0。
        /// </remarks>
        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", OtsExceptions.QueryTopValueOutOfRange);

                _top = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值表示查询时是否反向读取，即是否从大到小进行读取。
        /// </summary>
        /// <remarks>
        /// 默认值为False，即从小到大进行读取。
        /// </remarks>
        public bool IsReverse { get; set; }

        /// <summary>
        /// 初始化新的<see cref="OffsetRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// </exception>
        public OffsetRowQueryCriteria(string tableName)
            : this(tableName, null)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="OffsetRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param>
        /// <param name="viewName">要查询的视图（View）名。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// <para>- 或 -</para>
        /// <para>viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。</para>
        /// </exception>
        public OffsetRowQueryCriteria(string tableName, string viewName)
            : this(tableName, viewName, null, null)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="OffsetRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <param name="pagingKeys">分页键（Paging Key）名称与值的对应字典。</param>
        /// <param name="columnNames">需要返回列（Column）的名称的列表。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// </exception>
        public OffsetRowQueryCriteria(string tableName,
                                      IDictionary<string, PrimaryKeyValue> pagingKeys,
                                      IEnumerable<string> columnNames)
            : this(tableName, null, pagingKeys, columnNames)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="OffsetRowQueryCriteria"/>实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <param name="viewName">要查询的视图（View）名。</param>
        /// <param name="pagingKeys">分页键（Paging Key）名称与值的对应字典。</param>
        /// <param name="columnNames">需要返回列（Column）的名称的列表。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// <para>- 或 -</para>
        /// <para>viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。</para>
        /// </exception>
        public OffsetRowQueryCriteria(string tableName, string viewName,
            IDictionary<string, PrimaryKeyValue> pagingKeys, IEnumerable<string> columnNames)
            : base(tableName, viewName, columnNames)
        {
            if (pagingKeys != null)
            {
                foreach (var pk in pagingKeys)
                {
                    this.PagingKeys.Add(pk.Key, pk.Value);
                }
            }
        }
    }
}
