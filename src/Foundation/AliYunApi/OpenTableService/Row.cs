/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示行。
    /// </summary>
    /// <remarks>
    /// <para>表示查询时返回的数据行。<see cref="Columns"/>包含了查询时指定的返回列，可能有主键列也可能有属性列。
    /// 如果查询时没有指定返回列，则包含有整行所有列的数据。</para>
    /// <para>同时也可以使用该对象的枚举器枚举所有列的名称和值的对。</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class Row : IEnumerable<KeyValuePair<string, ColumnValue>>
    {
        private EntityDictionary<ColumnValue> _columns = new EntityDictionary<ColumnValue>();

        /// <summary>
        /// 获取列（Column）名称与值的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示列的值<see cref="ColumnValue"/>。</para>
        /// </remarks>
        public IDictionary<string, ColumnValue> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// 初始化新的<see cref="Row"/>实例。
        /// </summary>
        public Row()
        {
        }

        public IEnumerator<KeyValuePair<string, ColumnValue>> GetEnumerator()
        {
            return this.Columns.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

}
