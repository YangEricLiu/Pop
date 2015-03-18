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
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of GetRowCommand.
    /// </summary>
    internal class GetRowCommand : OtsCommand<GetRowResult>
    {
        private SingleRowQueryCriteria _criteria;
        private string _transactionId;
        
        public const string ActionName = "GetRow";
        
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
        
        protected GetRowCommand(IServiceClient client, Uri endpoint,
                             ExecutionContext context,
                             SingleRowQueryCriteria criteria, string transactionId)
            : base(client, endpoint, context)
        {
            Debug.Assert(criteria != null);
            _criteria = criteria;
            _transactionId = transactionId;
        }
        
        public static GetRowCommand Create(IServiceClient client, Uri endpoint,
                             ExecutionContext context,
                             SingleRowQueryCriteria criteria, string transactionId)
        {
            ValidateParameter(criteria);
            return new GetRowCommand(client, endpoint, context, criteria, transactionId);
        }
        
        private static void ValidateParameter(SingleRowQueryCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            if (criteria.PrimaryKeys.Count == 0)
                throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, "criteria");

            // The column name should have been verified when they're added to the criteria.
            OtsUtility.AssertColumnNames(criteria.PrimaryKeys.Keys);
            OtsUtility.AssertColumnNames(criteria.ColumnNames);
        }
        
        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableName", OtsUtility.GetFullQueryTableName(this._criteria));

            int pkIndex = 1;
            foreach (var pk in _criteria.PrimaryKeys)
            {
                var pkPre = string.Format(CultureInfo.InvariantCulture, "PK.{0}.", pkIndex++);
                parameters.Add(string.Concat(pkPre, "Name"), pk.Key);
                parameters.Add(string.Concat(pkPre, "Value"), pk.Value.ToParameterString());
                parameters.Add(string.Concat(pkPre, "Type"), PrimaryKeyTypeHelper.GetString(pk.Value.ValueType));
            }

            int colIndex = 1;
            foreach (var col in _criteria.ColumnNames)
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
