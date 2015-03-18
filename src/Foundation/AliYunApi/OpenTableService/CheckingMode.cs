/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示数据的存在性检查的方式。
    /// </summary>
    public enum CheckingMode
    {
        /// <summary>
        /// 不检查。
        /// </summary>
        No = 0,

        /// <summary>
        /// 检查Row必须不存在，即进行插入操作。
        /// </summary>
        Insert,

        /// <summary>
        /// 检查Row必须存在，即进行更新操作。
        /// </summary>
        Update
    }
}
