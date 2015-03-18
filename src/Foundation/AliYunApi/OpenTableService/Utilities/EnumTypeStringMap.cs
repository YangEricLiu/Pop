/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// A mapping that maps enum fields to their string representations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EnumTypeStringMap<T>
    {
        private IDictionary<string, T> _stringToTypeMap;
        private IDictionary<T, string> _typeToStringMap;

        public EnumTypeStringMap(IDictionary<T, string> typeToStringMap)
        {
            Debug.Assert(typeToStringMap != null);

            _typeToStringMap = typeToStringMap;

            _stringToTypeMap = new Dictionary<string, T>();
            foreach (var pair in typeToStringMap)
            {
                _stringToTypeMap.Add(pair.Value, pair.Key);
            }

            CheckMap();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic"), Conditional("DEBUG")]
        [DebuggerNonUserCode]
        private void CheckMap()
        {
            // Ensure all types have been added to the map.
            var t = typeof(T);
            Debug.Assert(t.IsEnum);
            var fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var f in fields)
            {
                Debug.Assert(_typeToStringMap.Keys.Select(k => k.ToString()).Contains(f.Name));
            }
        }

        public T GetEnumType(string value)
        {
            Debug.Assert(!string.IsNullOrEmpty(value));
            Debug.Assert(_stringToTypeMap.Keys.Contains(value));

            if (_stringToTypeMap.Keys.Contains(value))
            {
                return _stringToTypeMap[value];
            }
            else
            {
                // In case OTS adds new types in the future.
                return default(T);
            }
        }

        public string GetEnumString(T value)
        {
            Debug.Assert(_typeToStringMap.Keys.Contains(value));

            return _typeToStringMap[value];
        }
    }

    /// <summary>
    /// Helper for <see cref="PartitionKeyType"/>.
    /// </summary>
    internal static class PartitionKeyTypeHelper
    {
        private static object _lockObj = new object();

        private static EnumTypeStringMap<PartitionKeyType> _innerMap;

        public static string GetString(PartitionKeyType pkType)
        {
            EnsureMapCreated();
            return _innerMap.GetEnumString(pkType);
        }

        public static PartitionKeyType Parse(string value)
        {
            EnsureMapCreated();
            return _innerMap.GetEnumType(value);
        }

        private static void EnsureMapCreated()
        {
            if (_innerMap == null)
            {
                var map = new Dictionary<PartitionKeyType, string>()
                {
                    { PartitionKeyType.String, "STRING" },
                    { PartitionKeyType.Integer, "INTEGER" }
                };

                var innerMap = new EnumTypeStringMap<PartitionKeyType>(map);

                lock (_lockObj)
                {
                    if (_innerMap == null)
                    {
                        _innerMap = innerMap;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Helper for <see cref="PrimaryKeyType"/>.
    /// </summary>
    internal static class PrimaryKeyTypeHelper
    {
        private static object _lockObj = new object();
        private static EnumTypeStringMap<PrimaryKeyType> _innerMap;

        public static string GetString(PrimaryKeyType pkType)
        {
            EnsureMapCreated();
            return _innerMap.GetEnumString(pkType);
        }

        public static PrimaryKeyType Parse(string value)
        {
            EnsureMapCreated();
            return _innerMap.GetEnumType(value);
        }

        private static void EnsureMapCreated()
        {
            if (_innerMap == null)
            {
                var map = new Dictionary<PrimaryKeyType, string>()
                {
                    { PrimaryKeyType.String, "STRING" },
                    { PrimaryKeyType.Integer, "INTEGER" },
                    { PrimaryKeyType.Boolean, "BOOLEAN" }
                };

                var innerMap = new EnumTypeStringMap<PrimaryKeyType>(map);

                lock (_lockObj)
                {
                    if (_innerMap == null)
                    {
                        _innerMap = innerMap;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Helper for <see cref="ColumnType"/>.
    /// </summary>
    internal static class ColumnTypeHelper
    {
        private static object _lockObj = new object();
        private static EnumTypeStringMap<ColumnType> _innerMap;

        public static string GetString(ColumnType pkType)
        {
            EnsureMapCreated();
            return _innerMap.GetEnumString(pkType);
        }

        public static ColumnType Parse(string value)
        {
            EnsureMapCreated();
            return _innerMap.GetEnumType(value);
        }

        private static void EnsureMapCreated()
        {
            if (_innerMap == null)
            {
                var map = new Dictionary<ColumnType, string>()
                {
                    { ColumnType.String, "STRING" },
                    { ColumnType.Integer, "INTEGER" },
                    { ColumnType.Boolean, "BOOLEAN" },
                    { ColumnType.Double, "DOUBLE" }
                };

                var innerMap = new EnumTypeStringMap<ColumnType>(map);

                lock (_lockObj)
                {
                    if (_innerMap == null)
                    {
                        _innerMap = innerMap;
                    }
                }
            }
        }
    }
}
