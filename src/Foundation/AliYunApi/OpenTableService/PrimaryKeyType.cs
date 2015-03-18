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
    /// 表示主键（PrimaryKey）的数据类型。
    /// </summary>
    public enum PrimaryKeyType
    {
        /// <summary>
        /// 字符串型。
        /// </summary>
        String = 0,

        /// <summary>
        /// 64位整型。
        /// </summary>
        Integer = 1,

        /// <summary>
        /// 布尔型。
        /// </summary>
        Boolean = 2
    }
}
