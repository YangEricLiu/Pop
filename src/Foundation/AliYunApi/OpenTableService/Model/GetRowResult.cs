/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Model
{
    public class GetRowResult : OpenTableServiceResult
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlArray("Table")]
        [XmlArrayItem("Row")]
        public List<InternalTableRow> Rows { get; set; }

        public GetRowResult()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Row GetSingleRow()
        {
            if (Rows == null || Rows.Count == 0 ||
                (Rows.Count == 1 && Rows[0].Columns.Count == 0)) // An empty row
            {
                return null;
            }
            else
            {
                Debug.Assert(Rows.Count == 1);
                return Rows[0].ToRow();
            }
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<Row> GetMultipleRows()
        {
            return Rows != null ?
                Rows.Select(r => r.ToRow()) :
                new List<Row>();
        }
    }

    public class GetRowsByRangeResult : GetRowResult
    {
        public GetRowsByRangeResult()
        {
        }
    }

    public class GetRowsByOffsetResult : GetRowResult
    {
        public GetRowsByOffsetResult()
        {
        }
    }

    public class SqlQueryResult : GetRowResult
    {
        public SqlQueryResult()
        {
        }
    }

    [XmlRoot("Row")]
    public class InternalTableRow
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlElement("Column")]
        public List<InternalTableColumn> Columns { get; set; }

        public InternalTableRow()
        {
        }

        public Row ToRow()
        {
            var row = new Row();
            foreach (var col in this.Columns)
            {
                string decodedValue = null;

                if (!string.IsNullOrEmpty(col.Value.Encoding))
                {
                    switch (col.Value.Encoding)
                    {
                        case "Base64":
                            var bytes = Convert.FromBase64String(col.Value.Value);
                            try
                            {
                                decodedValue = OtsUtility.DataEncoding.GetString(bytes);
                            }
                            catch (DecoderFallbackException)
                            {
                                throw ExceptionFactory.CreateInvalidResponseException(
                                    null, OtsExceptions.ColumnValueCannotBeDecoded, null);
                            }
                            break;

                        default:
                            throw ExceptionFactory.CreateInvalidResponseException(
                                null,
                                string.Format(CultureInfo.CurrentUICulture, OtsExceptions.UnsupportedEncodingFormat, col.Value.Encoding),
                                null);
                    }
                }
                else
                {
                    decodedValue = col.Value.Value;
                }

                //if (col.IsPrimaryKey)
                //{
                //    row.PrimaryKeys[col.Name] = new PrimaryKeyValue(
                //         decodedValue, PrimaryKeyTypeHelper.Parse(col.Value.ColumnType));
                //}
                //else
                //{
                row.Columns[col.Name] = new ColumnValue(
                    decodedValue, ColumnTypeHelper.Parse(col.Value.ColumnType));
                //}
            }

            return row;
        }
    }

    [XmlRoot("Column")]
    [DebuggerDisplay("Column: {Name}")]
    public class InternalTableColumn
    {
        [XmlAttribute("PK")]
        public bool IsPrimaryKey { get; set; }

        public string Name { get; set; }

        public InternalTableColumnValue Value { get; set; }

        public InternalTableColumn()
        {
        }
    }

    [XmlRoot("Value")]
    [DebuggerDisplay("{ColumnType}: {Value}")]
    public class InternalTableColumnValue
    {
        [XmlAttribute("type")]
        public string ColumnType { get; set; }

        [XmlAttribute("encoding")]
        public string Encoding { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
