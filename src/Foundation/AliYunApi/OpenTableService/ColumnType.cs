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
    /// 表示表（Table）中列的数据类型。
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// 字符串型。默认值。
        /// </summary>
        String = 0,

        /// <summary>
        /// 64位带符号的整型。
        /// </summary>
        Integer = 1,

        /// <summary>
        /// 布尔型。
        /// </summary>
        Boolean = 2,

        /// <summary>
        /// 64位浮点型。
        /// </summary>
        Double = 3
    }
}
