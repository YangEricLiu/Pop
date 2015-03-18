/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Globalization;

using Aliyun.OpenServices.OpenTableService.Utilities;
using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示行的数据变更信息。
    /// </summary>
    public abstract class RowChange
    {
        /// <summary>
        /// 获取主键（Primary Key）列名称与值的对应字典。
        /// </summary>
        /// <remarks>
        /// <para>字典的键（Key）表示主键列的名称，由数字、英文字母和下划线构成，其中英文字母大小写敏感，数字不能作为第一个字符，名称的长度不能超过100个字符。</para>
        /// <para>字典的值（Value）表示主键列的值<see cref="PrimaryKeyValue"/>。</para>
        /// </remarks>
        public IDictionary<string, PrimaryKeyValue> PrimaryKeys { get; private set; }

        /// <summary>
        /// 获取变更方式。
        /// </summary>
        internal abstract string ModifyType { get; }

        /// <summary>
        /// 初始化新的<see cref="RowChange"/>对象。
        /// </summary>
        protected RowChange()
        {
            this.PrimaryKeys = new PrimaryKeyDictionary();
        }

        /// <summary>
        /// 初始化新的<see cref="RowChange"/>对象。
        /// </summary>
        /// <param name="primaryKeys">主键（Primary Key）列名称与值的对应字典。</param>
        protected RowChange(IDictionary<string, PrimaryKeyValue> primaryKeys)
            : this()
        {
            if (primaryKeys == null)
                throw new ArgumentNullException("primaryKeys");

            foreach (var pk in primaryKeys)
            {
                this.PrimaryKeys.Add(pk);
            }
        }
    }
}
