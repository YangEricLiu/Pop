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
using System.IO;
using System.Linq;

namespace Aliyun.OpenServices.OpenTableService.Commands.ProtoBuf
{
    /// <summary>
    /// Description of BatchModifyDataCommand.
    /// </summary>
    internal class BatchModifyDataCommand : OtsCommand
    {
        public const string ActionName = "BatchModifyRow";

        private string _tableName;

        private string _transactionId;

        private IEnumerable<RowChange> _rowChanges;

        protected override string ResourcePath
        {
            get
            {
                return ActionName;
            }
        }

        protected override HttpMethod Method
        {
            get
            {
                return HttpMethod.Post;
            }
        }

        protected BatchModifyDataCommand(IServiceClient client, Uri endpoint,
                                         ExecutionContext context,
                                         String tableName,
                                         IEnumerable<RowChange> rowChanges,
                                         string transactionId)
            : base(client, endpoint, context)
        {
            if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            if (string.IsNullOrEmpty(transactionId))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "transactionId");

            _tableName = tableName;
            _rowChanges = rowChanges;
            _transactionId = transactionId;
        }

        // Why hide the constructor and provide a factory method for each command?
        // Because it not only avoids time-consuming parameter validation in constructors,
        // but decouples construction from the specific classes that provides flexiblity to 
        // change implementation of each command.
        public static BatchModifyDataCommand Create(IServiceClient client, Uri endpoint,
                                                    ExecutionContext context,
                                                    String tableName,
                                                    IEnumerable<RowChange> rowChanges,
                                                    string transactionId)
        {
            ValidateRowChanges(rowChanges);

            return new BatchModifyDataCommand(client, endpoint, context,
                                              tableName, rowChanges, transactionId);
        }

        private static void ValidateRowChanges(IEnumerable<RowChange> rowChanges)
        {
            if (rowChanges == null)
                throw new ArgumentNullException("rowChanges");

            var rowChangeList = rowChanges.ToList();
            if (rowChangeList.Count == 0)
            {
                throw new ArgumentException(OtsExceptions.NoRowForBatchModifyData, "rowChanges");
            }

            foreach (var r in rowChangeList)
            {
                if (r.PrimaryKeys.Count == 0)
                {
                    throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "rowChanges");
                }
            }
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            var request = new BatchModifyRowRequest()
            {
                table_name = this._tableName,
                transaction_id = this._transactionId
            };

            List<ModifyItem> modifyItemList = new List<ModifyItem>();

            foreach (var row in this._rowChanges)
            {
                var modifyType = ProtoBufUtility.XmlModifyType2ProtoBufModifyType(row.ModifyType);

                var modifyItem = new ModifyItem()
                {
                    type = modifyType,
                    row_put_change = (modifyType == ModifyItem.ModifyType.PUT ? new OpenTableService.ProtoBuf.RowPutChange()
                    {
                        checking_type = OpenTableService.ProtoBuf.RowPutChange.CheckingType.NO,
                        row = ProtoBufUtility.XmlRowPutChange2ProtoBufRow((RowPutChange)row)
                    } : null),
                    row_delete_change = (modifyType == ModifyItem.ModifyType.DELETE ? new OpenTableService.ProtoBuf.RowDeleteChange()
                    {
                        primary_keys = row.PrimaryKeys.Select(pk => new Column()
                        {
                            name = pk.Key,
                            value = ProtoBufUtility.XmlPrimaryKeyValue2ProtoBufColumnValue(pk.Value)
                        }).ToList()

                    } : null)
                };

                modifyItemList.Add(modifyItem);
            }

            request.modify_items = modifyItemList;

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
