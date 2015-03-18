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
    /// Description of GetRowCommand.
    /// </summary>
    internal class GetRowCommand : OtsCommand<GetRowResponse>
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
            var requestParameter = new GetRowParameter()
                                        {
                                            table_name = OtsUtility.GetFullQueryTableName(_criteria),

                                            primary_keys = this._criteria.PrimaryKeys.Select(pk => new Column()
                                                                                                        {
                                                                                                            name = pk.Key,
                                                                                                            value = ProtoBufUtility.XmlPrimaryKeyValue2ProtoBufColumnValue(pk.Value)

                                                                                                        }).ToList(),

                                            column_names = _criteria.ColumnNames.ToList()
                                        };

            var request = new GetRowRequest()
                           {
                               get_row_parameter = requestParameter,
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