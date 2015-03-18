/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 表示分片键的数据类型。
    /// </summary>
    public enum PartitionKeyType
    {
        /// <summary>
        /// 字符串型。
        /// </summary>
        String = 0,

        /// <summary>
        /// 64位整型。
        /// </summary>
        Integer = 1
    }

}
