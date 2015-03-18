using Aliyun.OpenServices.OpenTableService.ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    public static class ProtoBufUtility
    {
        public static ProtoBuf.ColumnType XmlColumnType2ProtoBufColumnType(ColumnType xmlColumnType)
        {
            switch (xmlColumnType)
            {
                case ColumnType.Boolean:

                    return ProtoBuf.ColumnType.BOOLEAN;
                case ColumnType.Double:

                    return ProtoBuf.ColumnType.DOUBLE;
                case ColumnType.Integer:

                    return ProtoBuf.ColumnType.INTEGER;
                case ColumnType.String:
                default:

                    return ProtoBuf.ColumnType.STRING;
            }
        }

        public static ColumnType ProtoBufColumnType2XmlColumnType(ProtoBuf.ColumnType pbColumnType)
        {
            switch (pbColumnType)
            {
                case ProtoBuf.ColumnType.BOOLEAN:

                    return ColumnType.Boolean;
                case ProtoBuf.ColumnType.DOUBLE:

                    return ColumnType.Double;
                case ProtoBuf.ColumnType.INTEGER:

                    return ColumnType.Integer;
                case ProtoBuf.ColumnType.STRING:
                default:

                    return ColumnType.String;
            }
        }

        public static ProtoBuf.ColumnType XmlPrimaryKeyType2ProtoBufColumnType(PrimaryKeyType primaryKeyType)
        {
            switch (primaryKeyType)
            {
                case PrimaryKeyType.Boolean:

                    return ProtoBuf.ColumnType.BOOLEAN;
                case PrimaryKeyType.Integer:

                    return ProtoBuf.ColumnType.INTEGER;
                case PrimaryKeyType.String:
                default:

                    return ProtoBuf.ColumnType.STRING;
            }
        }

        public static PrimaryKeyType ProtoBufColumnType2XmlPrimaryType(ProtoBuf.ColumnType pbColumnType)
        {
            switch (pbColumnType)
            {
                case ProtoBuf.ColumnType.BOOLEAN:

                    return PrimaryKeyType.Boolean;
                case ProtoBuf.ColumnType.INTEGER:

                    return PrimaryKeyType.Integer;
                case ProtoBuf.ColumnType.STRING:
                default:

                    return PrimaryKeyType.String;
            }
        }

        public static ProtoBuf.ColumnValue XmlPrimaryKeyValue2ProtoBufColumnValue(PrimaryKeyValue xmlPrimaryKeyValue)
        {
            switch (xmlPrimaryKeyValue.ValueType)
            {
                case PrimaryKeyType.Boolean:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.BOOLEAN,
                        value_b = Convert.ToBoolean(xmlPrimaryKeyValue.Value)
                    };

                case PrimaryKeyType.Integer:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.INTEGER,
                        value_i = Convert.ToInt64(xmlPrimaryKeyValue.Value)
                    };

                case PrimaryKeyType.String:
                default:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.STRING,
                        value_s = xmlPrimaryKeyValue.Value
                    };
            }
        }

        public static ProtoBuf.ColumnType XmlPartitionKeyType2ProtoBufColumnType(PartitionKeyType xmlPartitionKeyType)
        {
            switch (xmlPartitionKeyType)
            {
                case PartitionKeyType.Integer:

                    return ProtoBuf.ColumnType.INTEGER;
                case PartitionKeyType.String:
                default:
                    return ProtoBuf.ColumnType.STRING;
            }
        }

        public static ProtoBuf.ColumnValue XmlPartitionKeyValue2ProtoBufColumnValue(PartitionKeyValue xmlPartitionKeyValue)
        {
            switch (xmlPartitionKeyValue.ValueType)
            {
                case PartitionKeyType.Integer:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.INTEGER,
                        value_i = Convert.ToInt64(xmlPartitionKeyValue.Value)
                    };

                case PartitionKeyType.String:
                default:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.STRING,
                        value_s = xmlPartitionKeyValue.Value
                    };
            }
        }

        public static string ProtoBufColumnValue2XmlColumnValue(ProtoBuf.ColumnValue pbColumnValue)
        {
            switch (pbColumnValue.type)
            {
                case ProtoBuf.ColumnType.BOOLEAN:
                    return pbColumnValue.value_b.ToString();

                case ProtoBuf.ColumnType.INTEGER:
                    return pbColumnValue.value_i.ToString();

                case ProtoBuf.ColumnType.DOUBLE:
                    return pbColumnValue.value_d.ToString();

                case ProtoBuf.ColumnType.STRING:
                default:
                    return pbColumnValue.value_s.ToString();
            }
        }

        public static ProtoBuf.ColumnValue XmlColumnValue2ProtoBufColumnValue(ColumnValue xmlColumnValue)
        {
            switch (xmlColumnValue.ValueType)
            {
                case ColumnType.Boolean:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.BOOLEAN,
                        value_b = Convert.ToBoolean(xmlColumnValue.Value)
                    };

                case ColumnType.Integer:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.INTEGER,
                        value_i = Convert.ToInt64(xmlColumnValue.Value)
                    };
                case ColumnType.Double:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.DOUBLE,
                        value_d = Convert.ToDouble(xmlColumnValue.Value)
                    };

                case ColumnType.String:
                default:
                    return new ProtoBuf.ColumnValue()
                    {
                        type = ProtoBuf.ColumnType.STRING,
                        value_s = xmlColumnValue.Value
                    };
            }
        }

        public static IEnumerable<Row> ProtoBufRows2XmlRows(IEnumerable<ProtoBuf.Row> pbRows)
        {
            return pbRows.Select(row => ProtoBufRow2XmlRow(row));
        }

        public static Row ProtoBufRow2XmlRow(ProtoBuf.Row pbRow)
        {
            var row = new Row();

            foreach (var pbPk in pbRow.primary_keys)
            {
                row.Columns[pbPk.name] = ProtoBufColumn2XmlColumn(pbPk);
            }

            foreach (var pbColumn in pbRow.columns)
            {
                row.Columns[pbColumn.name] = ProtoBufColumn2XmlColumn(pbColumn);
            }

            return row;
        }

        public static ColumnValue ProtoBufColumn2XmlColumn(ProtoBuf.Column pbColumn)
        {
            return new ColumnValue()
                       {
                           ValueType = ProtoBufColumnType2XmlColumnType(pbColumn.value.type),
                           Value = ProtoBufColumnValue2XmlColumnValue(pbColumn.value)
                       };

        }

        public static ModifyItem.ModifyType XmlModifyType2ProtoBufModifyType(string xmlModifyType)
        {
            switch (xmlModifyType)
            {
                case "PUT":
                default:
                    return ModifyItem.ModifyType.PUT;
                case "DELETE":
                    return ModifyItem.ModifyType.DELETE;
            }
        }

        public static ProtoBuf.Row XmlRowPutChange2ProtoBufRow(RowPutChange xmlRowPutChange)
        {
            var row = new ProtoBuf.Row();

            row.primary_keys = xmlRowPutChange.PrimaryKeys.Select(pk => new ProtoBuf.Column()
                                                                            {
                                                                                name = pk.Key,
                                                                                value = XmlPrimaryKeyValue2ProtoBufColumnValue(pk.Value)
                                                                            }).ToList();


            row.columns = xmlRowPutChange.AttributeColumns.Select(column => new ProtoBuf.Column()
                                                                                {
                                                                                    name = column.Key,
                                                                                    value = XmlColumnValue2ProtoBufColumnValue(column.Value)
                                                                                }).ToList();

            return row;
        }
    }
}