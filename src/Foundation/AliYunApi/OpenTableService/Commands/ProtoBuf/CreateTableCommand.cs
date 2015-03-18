/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.ProtoBuf;
using Aliyun.OpenServices.OpenTableService.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Commands.ProtoBuf
{
    /// <summary>
    /// The command to create a table.
    /// </summary>
    internal class CreateTableCommand : OtsCommand
    {
        private TableMeta _tableMeta;

        public const string ActionName = "CreateTable";

        protected override HttpMethod Method
        {
            get { return HttpMethod.Post; }
        }

        protected override string ResourcePath
        {
            get { return ActionName; }
        }

        protected CreateTableCommand(IServiceClient client,
                                     Uri endpoint,
                                     ExecutionContext context,
                                     TableMeta tableMeta)
            : base(client, endpoint, context)
        {
            Debug.Assert(tableMeta != null);
            _tableMeta = tableMeta;
        }

        public static CreateTableCommand Create(IServiceClient client,
                                                Uri endpoint,
                                                ExecutionContext context,
                                                TableMeta tableMeta)
        {
            ValidateTableMeta(tableMeta);
            return new CreateTableCommand(client, endpoint, context, tableMeta);
        }

        private static void ValidateTableMeta(TableMeta tableMeta)
        {
            const string paramName = "tableMeta";

            if (tableMeta == null)
                throw new ArgumentNullException(paramName);

            if (string.IsNullOrEmpty(tableMeta.TableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, paramName);

            if (tableMeta.PrimaryKeys.Count == 0)
                throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, paramName);

            // The primary key names should be verified when they are added to table meta.
            OtsUtility.AssertColumnNames(tableMeta.PrimaryKeys.Keys);

            if (tableMeta.PagingKeyLength < 0 ||
                tableMeta.PagingKeyLength >= tableMeta.PrimaryKeys.Count)
            {
                throw new ArgumentException(OtsExceptions.InvalidPagingKeyLength, paramName);
            }

            foreach (var view in tableMeta.Views)
            {
                Debug.Assert(OtsUtility.IsEntityNameValid(view.ViewName),
                             "ViewName should have been validated when it is added.");

                OtsUtility.AssertColumnNames(view.PrimaryKeys.Keys);
                OtsUtility.AssertColumnNames(view.AttributeColumns.Keys);

                if (view.PagingKeyLength < 0 ||
                    view.PagingKeyLength >= view.PrimaryKeys.Count)
                {
                    throw new ArgumentException(OtsExceptions.InvalidPagingKeyLength, paramName);
                }
            }
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            var tableMeta = new OpenTableService.ProtoBuf.TableMeta()
                                {
                                    table_name = this._tableMeta.TableName,
                                    primary_keys = this._tableMeta.PrimaryKeys.Select(pk => new ColumnSchema()
                                                                                                {
                                                                                                    name = pk.Key,
                                                                                                    type = ProtoBufUtility.XmlPrimaryKeyType2ProtoBufColumnType(pk.Value)

                                                                                                }).ToList(),

                                    views = this._tableMeta.Views.Select(view => new OpenTableService.ProtoBuf.ViewMeta()
                                                                                    {
                                                                                        view_name = view.ViewName,
                                                                                        primary_keys = view.PrimaryKeys.Select(pk => new ColumnSchema()
                                                                                                                                        {
                                                                                                                                            name = pk.Key,
                                                                                                                                            type = ProtoBufUtility.XmlPrimaryKeyType2ProtoBufColumnType(pk.Value)

                                                                                                                                        }).ToList(),
                                                                                        columns = view.AttributeColumns.Select(column => new ColumnSchema()
                                                                                                                                        {
                                                                                                                                            name = column.Key,
                                                                                                                                            type = ProtoBufUtility.XmlColumnType2ProtoBufColumnType(column.Value)

                                                                                                                                        }).ToList()

                                                                                    }).ToList(),
                                    table_group_name = this._tableMeta.TableGroupName

                                };


            var request = new CreateTableRequest()
                            {
                                table_meta = tableMeta
                            };

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, request);

                var content = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(content, 0, content.Length);

                parameters.Add(ProtoBufConstant.ParameterContent, string.Join(ProtoBufConstant.Separator.ToString(), content));
            }
        }
    }
}