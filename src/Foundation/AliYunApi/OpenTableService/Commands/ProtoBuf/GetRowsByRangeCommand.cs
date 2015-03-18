/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Model;
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
    /// Description of GetRowsByRangeCommand.
    /// </summary>
    internal class GetRowsByRangeCommand : OtsCommand<GetRowsByRangeResponse>
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
            var request = new GetRowsByRangeRequest()
                              {
                                  table_name = OtsUtility.GetFullQueryTableName(_criteria),
                                  primary_key_prefix = this._criteria.PrimaryKeys.Select(pk => new Column()
                                                                                                   {
                                                                                                       name = pk.Key,
                                                                                                       value = ProtoBufUtility.XmlPrimaryKeyValue2ProtoBufColumnValue(pk.Value)

                                                                                                   }).ToList(),
                                  range_key_name = _criteria.Range.PrimaryKeyName,
                                  range_begin = ProtoBufUtility.XmlPrimaryKeyValue2ProtoBufColumnValue(_criteria.Range.RangeBegin),
                                  range_end = ProtoBufUtility.XmlPrimaryKeyValue2ProtoBufColumnValue(_criteria.Range.RangeEnd),
                                  column_names = _criteria.ColumnNames.ToList(),
                                  is_reverse = _criteria.IsReverse,
                                  limit = Convert.ToUInt32(_criteria.Top),
                                  transaction_id = _transactionId
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