/*
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
    /// 表示获取行的查询条件。
    /// </summary>
    public abstract class RowQueryCriteria
    {
        private string _tableName;
        private string _viewName;
        private IList<string> _columns = new EntityNameList();

        /// <summary>
        /// 获取或设置要查询的表（Table）名。
        /// </summary>
        /// <remarks>
        /// 名称由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
        /// </remarks>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || !OtsUtility.IsEntityNameValid(value))
                    throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");

                _tableName = value;
            }
        }

        /// <summary>
        /// 获取或设置要查询的视图（View）名。如果该属性未设置或设置为空引用或空字符串，则从表中查询。
        /// </summary>
        /// <remarks>
        /// 名称由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。
        /// </remarks>
        public string ViewName
        {
            get
            {
                return _viewName;
            }
            set
            {
                // viewName can be set to null.
                if (!OtsUtility.IsEntityNameValid(value))
                    throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");

                _viewName = value;
            }
        }

        /// <summary>
        /// 获取需要返回列（Column）的名称的列表。
        /// </summary>
        /// <remarks>
        /// <para>列的名称由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>如果列表中指定了要返回的列的名称，则返回的结果中将只包含指定列的数据。否则，如果列表为空，则返回结果将包含行中的所有列的数据。</para>
        /// </remarks>
        public IList<string> ColumnNames
        {
            get
            {
                return _columns;
            }
        }

        /// <summary>
        /// 初始化新的<see cref="RowQueryCriteria" />实例。
        /// </summary>
        /// <param name="tableName">要查询的表（Table）名。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// </exception>
        protected RowQueryCriteria(string tableName)
            : this(tableName, (string)null)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="RowQueryCriteria" />实例。
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
        protected RowQueryCriteria(string tableName, string viewName)
            : this(tableName, viewName, null)
        {
            _tableName = tableName;
            _viewName = viewName;
        }

        /// <summary>
        /// 初始化新的<see cref="RowQueryCriteria" />实例。
        /// </summary>
        /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param>
        /// <param name="columnNames">需要返回列（Column）的名称的列表。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// <para>- 或 -</para>
        /// <para>viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">columnNames为空引用。</exception>
        protected RowQueryCriteria(string tableName, IEnumerable<string> columnNames)
            : this(tableName, null, columnNames)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="RowQueryCriteria" />实例。
        /// </summary>
        /// <param name="tableName">与要查询的视图（View）相关的表（Table）名。</param>
        /// <param name="viewName">要查询的视图（View）名。</param>
        /// <param name="columnNames">需要返回列（Column）的名称的列表。</param>
        /// <exception cref="ArgumentException">
        /// <para>tableName为空引用或值为空字符串，</para>
        /// <para>- 或 -</para>
        /// <para>tableName不符合OTS名称的命名规范。</para>
        /// <para>- 或 -</para>
        /// <para>viewName不为空引用或值为空字符串但不符合OTS名称的命名规范。</para>
        /// </exception>
        protected RowQueryCriteria(string tableName, string viewName, IEnumerable<string> columnNames)
        {
            if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            // viewName could be null or empty.
            if (!string.IsNullOrEmpty(viewName) && !OtsUtility.IsEntityNameValid(viewName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "viewName");

            _tableName = tableName;
            _viewName = viewName;

            if (columnNames != null)
            {
                foreach (var col in columnNames)
                {
                    this.ColumnNames.Add(col);
                }
            }
        }
    }
}
