/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of BatchModifyDataCommand.
    /// </summary>
    internal class BatchModifyDataCommand : OtsCommand
    {
        public const string ActionName = "BatchModifyData";

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
            parameters.Add("TableName", _tableName);

            var itemIndex = 1;
            foreach (var rowChange in _rowChanges)
            {
                var pre = string.Format(CultureInfo.InvariantCulture, "Modify.{0}.", itemIndex++);

                parameters.Add(string.Concat(pre, "Type"), rowChange.ModifyType.ToString().ToUpperInvariant());

                var pkIndex = 1;
                foreach (var pk in rowChange.PrimaryKeys)
                {
                    var pkPre = string.Format(CultureInfo.InvariantCulture, "{0}PK.{1}.", pre, pkIndex++);
                    parameters.Add(string.Concat(pkPre, "Name"), pk.Key);
                    parameters.Add(string.Concat(pkPre, "Value"), pk.Value.ToParameterString());
                    parameters.Add(string.Concat(pkPre, "Type"), PrimaryKeyTypeHelper.GetString(pk.Value.ValueType));
                }

                var colIndex = 1;

                var rowPutChange = rowChange as RowPutChange;
                if (rowPutChange != null)
                {
                    foreach (var col in rowPutChange.AttributeColumns)
                    {
                        var colPre = string.Format(CultureInfo.InvariantCulture, "{0}Column.{1}.", pre, colIndex++);
                        parameters.Add(string.Concat(colPre, "Name"), col.Key);
                        parameters.Add(string.Concat(colPre, "Value"), col.Value.ToParameterString());
                        parameters.Add(string.Concat(colPre, "Type"), ColumnTypeHelper.GetString(col.Value.ValueType));
                    }

                    parameters.Add(string.Concat(pre, "Checking"),
                                   rowPutChange.CheckingMode.ToString().ToUpperInvariant());
                }
                else
                {
                    var rowDelChange = rowChange as RowDeleteChange;
                    foreach (var col in rowDelChange.ColumnNames)
                    {
                        var colName = string.Format(CultureInfo.InvariantCulture, "{0}Column.{1}.Name", pre, colIndex++);
                        parameters.Add(colName, col);
                    }
                }
            }

            parameters.Add("TransactionID", _transactionId);
        }

    }
}
