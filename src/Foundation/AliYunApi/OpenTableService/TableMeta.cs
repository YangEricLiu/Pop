/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.OpenTableService.Utilities;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示表（Table）的结构信息。
    /// </summary>
    public class TableMeta
    {
        private string _tableName;
        private string _tableGroupName;
        private EntityDictionary<PrimaryKeyType> _primaryKeys;

        /// <summary>
        /// 获取或设置表（Table）名。
        /// </summary>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.NamingRules.Remarks"]/*'/>
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
        /// 获取或设置表组（Table Group）名。
        /// 如果该属性未设置或设置为空引用或值为空字符串，则该表不创建在表组中。
        /// </summary>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.NamingRules.Remarks"]/*'/>
        public string TableGroupName
        {
            get
            {
                return _tableGroupName;
            }
            set
            {
                // It is allowed to set TableGroupName to null.
                if (!string.IsNullOrEmpty(value) && !OtsUtility.IsEntityNameValid(value))
                {
                        throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");
                }

                _tableGroupName = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，表示分页建立在前几个主键（Primary Key）列上。
        /// </summary>
        /// <remarks>此参数的值必须小于主键列的个数，如果未设置或设置为0表示不建立分页键。</remarks>
        public int PagingKeyLength { get; set; }

        /// <summary>
        /// 获取主键列的名称与数据类型的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示主键列的数据类型<see cref="PrimaryKeyType"/>。</para>
        /// </remarks>
        public IDictionary<string, PrimaryKeyType> PrimaryKeys
        {
            get { return _primaryKeys; }
        }

        /// <summary>
        /// 获取与该表（Table）相关的视图（View）的列表。
        /// </summary>
        public IList<ViewMeta> Views { get; private set; }

        // To avoid invalid table names, user has to construct a TableMeta with a table name.
        private TableMeta()
        {
            _primaryKeys = new EntityDictionary<PrimaryKeyType>();
            this.Views = new List<ViewMeta>();
        }

        /// <summary>
        /// 初始化新的<see cref="TableMeta"/>实例。
        /// </summary>
        /// <param name="tableName">表（Table）名。</param>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.NamingRules.ArgumentException"]/*'/>
        public TableMeta(string tableName)
            : this()
        {
            if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            TableName = tableName;
        }

        /// <summary>
        /// 初始化新的<see cref="TableMeta"/>实例。
        /// </summary>
        /// <param name="tableName">表（Table）名。</param>
        /// <param name="primaryKeys">主键列名称与数据类型的对应字典。</param>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.NamingRules.ArgumentException"]/*'/>
        /// <exception cref="ArgumentNullException">primaryKeys为空引用。</exception>
        public TableMeta(string tableName, IDictionary<string, PrimaryKeyType> primaryKeys)
        {
            if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            if (primaryKeys == null)
                throw new ArgumentNullException("primaryKeys");

            TableName = tableName;
            _primaryKeys = new EntityDictionary<PrimaryKeyType>(primaryKeys);
            this.Views = new List<ViewMeta>();
        }
    }

    /// <summary>
    /// 表示视图（View）的结构信息。
    /// </summary>
    public class ViewMeta
    {
        private string _viewName;
        private EntityDictionary<PrimaryKeyType> _primaryKeys;
        private EntityDictionary<ColumnType> _columns;

        /// <summary>
        /// 获取或设置视图（View）名。
        /// </summary>
        /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.NamingRules.Remarks"]/*'/>
        public string ViewName
        {
            get
            {
                return _viewName;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || !OtsUtility.IsEntityNameValid(value))
                    throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "value");

                _viewName = value;
            }
        }

        /// <summary>
        /// 获取主键列的名称与数据类型的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示主键列的数据类型<see cref="PrimaryKeyType"/>。</para>
        /// </remarks>
        public IDictionary<string, PrimaryKeyType> PrimaryKeys
        {
            get { return _primaryKeys; }
        }

        /// <summary>
        /// 获取属性列的名称与数据类型的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示主键列的数据类型<see cref="ColumnType"/>。</para>
        /// </remarks>
        public IDictionary<string, ColumnType> AttributeColumns
        {
            get { return _columns; }
        }

        /// <summary>
        /// 获取或设置一个值表示在当前视图上前几个主键列（Primary Key）建立分页（Paging）。
        /// </summary>
        /// <remarks>
        /// 此参数的值必须小于主键列的个数，如果设置为0表示不建立分页键。
        /// </remarks>
        public int PagingKeyLength { get; set; }

        private ViewMeta()
        {
            _primaryKeys = new EntityDictionary<PrimaryKeyType>();
            _columns = new EntityDictionary<ColumnType>();
        }

        /// <summary>
        /// 初始化新的<see cref="ViewMeta"/>实例。
        /// </summary>
        /// <param name="viewName">视图（View）名。</param>
        public ViewMeta(string viewName)
            : this()
        {
            if (string.IsNullOrEmpty(viewName) || !OtsUtility.IsEntityNameValid(viewName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "viewName");

            ViewName = viewName;
        }
    }
}
