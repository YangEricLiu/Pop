/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Model;
using Aliyun.OpenServices.OpenTableService.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of GetRowsByRangeCommand.
    /// </summary>
    internal class GetRowsByRangeCommand : OtsCommand<GetRowsByRangeResult>
    {
        private RangeRowQueryCriteria _criteria;
        private string _transactionId;

        public const string ActionName = "GetRowsByRange";

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

        protected GetRowsByRangeCommand(IServiceClient client, Uri endpoint,
                             ExecutionContext context,
                             RangeRowQueryCriteria criteria, string transactionId)
            : base(client, endpoint, context)
        {
            Debug.Assert(criteria != null);
            _criteria = criteria;
            _transactionId = transactionId;
        }

        public static GetRowsByRangeCommand Create(IServiceClient client, Uri endpoint,
                             ExecutionContext context,
                             RangeRowQueryCriteria criteria, string transactionId)
        {
            ValidateParameter(criteria);
            return new GetRowsByRangeCommand(client, endpoint, context, criteria, transactionId);
        }

        private static void ValidateParameter(RangeRowQueryCriteria criteria)
        {
            const string paramName = "criteria";

            if (criteria == null)
                throw new ArgumentNullException(paramName);

            OtsUtility.AssertColumnNames(criteria.PrimaryKeys.Keys);
            OtsUtility.AssertColumnNames(criteria.ColumnNames);

            if (criteria.Range == null)
                throw new ArgumentException(OtsExceptions.RangeNotSet, paramName);

            PrimaryKeyValue rangeBegin = criteria.Range.RangeBegin;
            PrimaryKeyValue rangeEnd = criteria.Range.RangeEnd;

            if ((!criteria.IsReverse && (rangeBegin == PrimaryKeyRange.InfMax || rangeEnd == PrimaryKeyRange.InfMin) ||
                 (criteria.IsReverse && (rangeBegin == PrimaryKeyRange.InfMin || rangeEnd == PrimaryKeyRange.InfMax))))
                throw new ArgumentException(OtsExceptions.PKInfNotAllowed);

            if ((!criteria.IsReverse && rangeBegin.CompareTo(rangeEnd) > 0) ||
                (criteria.IsReverse && rangeBegin.CompareTo(rangeEnd) < 0))
                throw new ArgumentException(OtsExceptions.RangeBeginGreaterThanRangeEnd);

        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableName", OtsUtility.GetFullQueryTableName(_criteria));

            int pkIndex = 1;
            foreach (var pk in _criteria.PrimaryKeys)
            {
                var prefix = string.Format(CultureInfo.InvariantCulture, "PK.{0}.", pkIndex++);
                parameters.Add(string.Concat(prefix, "Name"), pk.Key);
                parameters.Add(string.Concat(prefix, "Value"), pk.Value.ToParameterString());
                parameters.Add(string.Concat(prefix, "Type"), PrimaryKeyTypeHelper.GetString(pk.Value.ValueType));
            }

            var rangePrefix = string.Format(CultureInfo.InvariantCulture, "PK.{0}.", pkIndex);
            parameters.Add(string.Concat(rangePrefix, "Name"), _criteria.Range.PrimaryKeyName);
            parameters.Add(string.Concat(rangePrefix, "RangeBegin"), _criteria.Range.RangeBegin.ToParameterString());
            parameters.Add(string.Concat(rangePrefix, "RangeEnd"), _criteria.Range.RangeEnd.ToParameterString());
            parameters.Add(string.Concat(rangePrefix, "RangeType"), PrimaryKeyTypeHelper.GetString(_criteria.Range.RangeBegin.ValueType));

            int colIndex = 1;
            foreach (var col in _criteria.ColumnNames)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "Column.{0}.Name", colIndex++),
                               col);
            }

            if (_criteria.Top >= 0)
            {
                parameters.Add("Top", _criteria.Top.ToString(CultureInfo.InvariantCulture));
            }

            if (_criteria.IsReverse)
            {
                parameters.Add("IsReverse", _criteria.IsReverse.ToString(CultureInfo.InvariantCulture).ToUpperInvariant());
            }

            if (!string.IsNullOrEmpty(_transactionId))
            {
                parameters.Add("TransactionID", _transactionId);
            }
        }

    }
}




