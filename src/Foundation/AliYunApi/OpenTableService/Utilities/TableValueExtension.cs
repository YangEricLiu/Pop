/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    internal static class TableValueExtension
    {
        internal static string ToParameterString(this PrimaryKeyValue value)
        {
            switch (value.ValueType)
            {
                case PrimaryKeyType.String:
                    if (value.IsInf)
                    {
                        // INF_MAX or INF_MIN should not be quoted.
                        return value.ToString();
                    }
                    else
                    {
                        return string.Concat("'", value.ToString(), "'");
                    }

                case PrimaryKeyType.Boolean:
                    return value.Value.ToUpperInvariant();

                default:
                    return value.Value;
            }
        }

        internal static string ToParameterString(this ColumnValue value)
        {
            switch (value.ValueType)
            {
                case ColumnType.String:
                    return string.Concat("'", value.ToString(), "'");

                case ColumnType.Boolean:
                    return value.Value.ToUpperInvariant();

                default:
                    return value.Value;
            }
        }

        internal static string ToParameterString(this PartitionKeyValue value)
        {
            switch (value.ValueType)
            {
                case PartitionKeyType.String:
                    return string.Concat("'", value.ToString(), "'");

                default:
                    return value.Value;
            }
        }

    }

}
