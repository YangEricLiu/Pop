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
    /// 表示数据行的删除信息。
    /// </summary>
    public class RowDeleteChange : RowChange
    {
        /// <summary>
        /// 获取变更方式。
        /// </summary>
        internal override string ModifyType
        {
            get { return "DELETE"; }
        }

        /// <summary>
        /// 获取要删除属性列（Attribute Column）的名称列表，如果该列表为空则删除整行。
        /// </summary>
        public ICollection<string> ColumnNames { get; private set; }

        /// <summary>
        /// 初始化新的<see cref="RowDeleteChange"/>实例。
        /// </summary>
        public RowDeleteChange()
            : base()
        {
            InitializeColumns();
        }

        /// <summary>
        /// 初始化新的<see cref="RowDeleteChange"/>实例。
        /// </summary>
        /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
        public RowDeleteChange(IDictionary<string, PrimaryKeyValue> primaryKeys)
            : base(primaryKeys)
        {
            InitializeColumns();
        }

        private void InitializeColumns()
        {
            ColumnNames = new EntityNameList();
        }

        /// <summary>
        /// 初始化新的<see cref="RowDeleteChange"/>实例。
        /// </summary>
        /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
        /// <param name="columnNames">要删除属性列（Attribute Column）的名称列表。</param>
        public RowDeleteChange(IDictionary<string, PrimaryKeyValue> primaryKeys, IEnumerable<string> columnNames)
            : this(primaryKeys)
        {
            if (columnNames == null)
                throw new ArgumentNullException("columnNames");

            this.ColumnNames = new EntityNameList(columnNames);
        }
    }

}
