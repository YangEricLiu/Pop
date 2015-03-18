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
    /// 表示行的插入或更新信息。
    /// </summary>
    /// <include file='ots_comments_include.xml' path='Comments/Member[@name="OpenTableService.SampleCode.PutData"]/*'/>
    public class RowPutChange : RowChange
    {
        /// <summary>
        /// 获取变更方式。
        /// </summary>
        internal override string ModifyType
        {
            get { return "PUT"; }
        }

        /// <summary>
        /// 获取或设置进行数据存在性检查的方式。
        /// </summary>
        public CheckingMode CheckingMode { get; set; }

        /// <summary>
        /// 获取属性列（Attribute Column）名称与值的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示列的值<see cref="ColumnValue"/>。</para>
        /// </remarks>
        public IDictionary<string, ColumnValue> AttributeColumns { get; private set; }

        /// <summary>
        /// 初始化新的<see cref="RowPutChange"/>实例。
        /// </summary>
        public RowPutChange()
            : base()
        {
            this.AttributeColumns = new EntityDictionary<ColumnValue>();
        }

        /// <summary>
        /// 初始化新的<see cref="RowPutChange"/>实例。
        /// </summary>
        /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
        public RowPutChange(IDictionary<string, PrimaryKeyValue> primaryKeys)
            : base(primaryKeys)
        {
            this.AttributeColumns = new EntityDictionary<ColumnValue>();
        }

        /// <summary>
        /// 初始化新的<see cref="RowPutChange"/>实例。
        /// </summary>
        /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
        /// <param name="columns">属性列（Attribute Column）名称与值的对应字典。</param>
        public RowPutChange(IDictionary<string, PrimaryKeyValue> primaryKeys, IDictionary<string, ColumnValue> columns)
            : base(primaryKeys)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            this.AttributeColumns = new EntityDictionary<ColumnValue>(columns);
        }
    }
}
