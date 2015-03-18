/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Globalization;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of PutDataCommand.
    /// </summary>
    internal class PutDataCommand : OtsCommand
    {
        private string _tableName;

        private string _transactionId;

        private RowPutChange _rowChange;

        public const string ActionName = "PutData";

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

        protected PutDataCommand(IServiceClient client, Uri endpoint,
                              ExecutionContext context,
                              String tableName, RowPutChange rowChange, string transactionId)
            : base(client, endpoint, context)
        {
            _tableName = tableName;
            _rowChange = rowChange;
            _transactionId = transactionId;
        }

        public static PutDataCommand Create(IServiceClient client, Uri endpoint,
                              ExecutionContext context,
                              String tableName, RowPutChange rowChange, string transactionId)
        {
            if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            ValidateRowChange(rowChange);
            return new PutDataCommand(client, endpoint, context, tableName, rowChange, transactionId);
        }

        private static void ValidateRowChange(RowPutChange rowChange)
        {
            const string paramName = "rowChange";

            if (rowChange == null)
                throw new ArgumentNullException(paramName);

            if (rowChange.PrimaryKeys.Count == 0)
            {
                throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, paramName);
            }

            OtsUtility.AssertColumnNames(rowChange.PrimaryKeys.Keys);
            OtsUtility.AssertColumnNames(rowChange.AttributeColumns.Keys);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableName", _tableName);

            var pkIndex = 1;
            foreach (var pk in _rowChange.PrimaryKeys)
            {
                var pre = string.Format(CultureInfo.InvariantCulture, "PK.{0}.", pkIndex++);
                parameters.Add(string.Concat(pre, "Name"), pk.Key);
                parameters.Add(string.Concat(pre, "Value"), pk.Value.ToParameterString());
                parameters.Add(string.Concat(pre, "Type"), PrimaryKeyTypeHelper.GetString(pk.Value.ValueType));
            }

            var colIndex = 1;
            foreach (var col in _rowChange.AttributeColumns)
            {
                var pre = string.Format(CultureInfo.InvariantCulture, "Column.{0}.", colIndex++);
                parameters.Add(string.Concat(pre, "Name"), col.Key);
                parameters.Add(string.Concat(pre, "Value"), col.Value.ToParameterString());
                parameters.Add(string.Concat(pre, "Type"), ColumnTypeHelper.GetString(col.Value.ValueType));
            }

            parameters.Add("Checking", _rowChange.CheckingMode.ToString().ToUpperInvariant());

            if (!string.IsNullOrEmpty(_transactionId))
            {
                parameters.Add("TransactionID", _transactionId);
            }
        }
    }
}
