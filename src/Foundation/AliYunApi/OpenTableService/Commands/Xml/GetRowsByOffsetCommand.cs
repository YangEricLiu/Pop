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
    /// Description of GetRowsByOffsetCommand.
    /// </summary>
    internal class GetRowsByOffsetCommand : OtsCommand<GetRowsByOffsetResult>
    {
        private OffsetRowQueryCriteria _criteria;
        private string _transactionId;

        public const string ActionName = "GetRowsByOffset";

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

        protected GetRowsByOffsetCommand(IServiceClient client, Uri endpoint,
                                      ExecutionContext context,
                                      OffsetRowQueryCriteria criteria, string transactionId)
            : base(client, endpoint, context)
        {
            Debug.Assert(criteria != null);
            _criteria = criteria;
            _transactionId = transactionId;
        }

        public static GetRowsByOffsetCommand Create(IServiceClient client, Uri endpoint,
                                                    ExecutionContext context,
                                                    OffsetRowQueryCriteria criteria, string transactionId)
        {
            ValidateParameter(criteria);
            return new GetRowsByOffsetCommand(client, endpoint, context, criteria, transactionId);
        }

        private static void ValidateParameter(OffsetRowQueryCriteria criteria)
        {
            const string paramName = "criteria";

            if (criteria == null)
                throw new ArgumentNullException(paramName);

            if (criteria.PagingKeys.Count == 0)
                throw new ArgumentException(OtsExceptions.NoPrimaryKeySpecified, paramName);

            if (!criteria.isTopSet)
                throw new ArgumentException(OtsExceptions.QueryTopValueNotSet, paramName);

            OtsUtility.AssertColumnNames(criteria.PagingKeys.Keys);
            OtsUtility.AssertColumnNames(criteria.ColumnNames);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableName", OtsUtility.GetFullQueryTableName(_criteria));

            int pkIndex = 1;
            foreach (var pk in _criteria.PagingKeys)
            {
                var pkPre = string.Format(CultureInfo.InvariantCulture, "Paging.{0}.", pkIndex++);
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
            parameters.Add("Offset", _criteria.Offset.ToString(CultureInfo.InvariantCulture));
            parameters.Add("Top", _criteria.Top.ToString(CultureInfo.InvariantCulture));

            if (_criteria.IsReverse){
                parameters.Add("IsReverse", _criteria.IsReverse.ToString(CultureInfo.InvariantCulture).ToUpperInvariant());
            }

            if (!string.IsNullOrEmpty(_transactionId))
            {
                parameters.Add("TransactionID", _transactionId);
            }
        }

    }
}
