/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;


namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// Helper class.
    /// </summary>
    internal static class OtsUtility
    {
        public static readonly Encoding DataEncoding = Encoding.UTF8;

        public static bool IsEntityNameValid(string name)
        {
            // The caller is responsible for checking name is not null or empty.
            Debug.Assert(!string.IsNullOrEmpty(name));
            var pattern = @"^[a-zA-Z_][\w]{0,99}$";
            var regex = new Regex(pattern);
            var m = regex.Match(name);
            return m.Success;
        }

        /// <summary>
        /// Gets the TableName parameter for GetRow(xYz).
        /// If ViewName is set, returns the full view name. Otherwise, returns the TableName.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static string GetFullQueryTableName(RowQueryCriteria criteria)
        {
            Debug.Assert(criteria != null);
            Debug.Assert(!string.IsNullOrEmpty(criteria.TableName) && IsEntityNameValid(criteria.TableName));

            var entityName = criteria.TableName;

            if (!string.IsNullOrEmpty(criteria.ViewName))
            {
                Debug.Assert(IsEntityNameValid(criteria.ViewName));
                entityName += "." + criteria.ViewName;
            }

            return entityName;
        }

        [Conditional("DEBUG")]
        [DebuggerNonUserCode]
        public static void AssertColumnNames(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                Debug.Assert(OtsUtility.IsEntityNameValid(name));
            }
        }
    }
}