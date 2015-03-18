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

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of DeleteDataCommand.
    /// </summary>
    internal class DeleteDataCommand : OtsCommand
    {
        private string _tableName;

        private string _transactionId;

        private RowDeleteChange _rowChange;

        public const string ActionName = "DeleteData";

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

        protected DeleteDataCommand(IServiceClient client, Uri endpoint,
                              ExecutionContext context,
                              String tableName, RowDeleteChange rowChange, string transactionId)
            : base(client, endpoint, context)
        {
            Debug.Assert(!string.IsNullOrEmpty(tableName)
                         && OtsUtility.IsEntityNameValid(tableName)
                         && rowChange != null);

            _tableName = tableName;
            _rowChange = rowChange;
            _transactionId = transactionId;
        }

        public static DeleteDataCommand Create(IServiceClient client, Uri endpoint,
                              ExecutionContext context,
                              String tableName, RowDeleteChange rowChange, string transactionId)
        {
            if (string.IsNullOrEmpty(tableName) || !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            ValidateRowChange(rowChange);
            return new DeleteDataCommand(client, endpoint, context, tableName, rowChange, transactionId);
        }

        private static void ValidateRowChange(RowDeleteChange rowChange)
        {
            const string paramName = "rowChange";

            if (rowChange == null)
                throw new ArgumentNullException(paramName);

            if (rowChange.PrimaryKeys.Count == 0)
            {
                throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, paramName);
            }

            OtsUtility.AssertColumnNames(rowChange.PrimaryKeys.Keys);
            OtsUtility.AssertColumnNames(rowChange.ColumnNames);
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
            foreach (var col in _rowChange.ColumnNames)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "Column.{0}.Name", colIndex++),
                               col);
            }

            if (!string.IsNullOrEmpty(_transactionId))
            {
                parameters.Add("TransactionID", _transactionId);
            }
        }
    }
}
