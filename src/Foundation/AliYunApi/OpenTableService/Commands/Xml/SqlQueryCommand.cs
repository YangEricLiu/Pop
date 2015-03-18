/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Model;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    internal class SqlQueryCommand : OtsCommand<SqlQueryResult>
    {
        public const string ActionName = "SqlQuery";

        private string _selectExpression;
        private string _transactionId;

        protected override string ResourcePath
        {
            get { return ActionName; }
        }
        
        protected override HttpMethod Method
        {
            get { return HttpMethod.Post; }
        }

        public SqlQueryCommand(IServiceClient client, Uri endpoint,
                               ExecutionContext context,
                               String selectExpression, String transactionId)
            : base(client, endpoint, context)
        {
            _selectExpression = selectExpression;
            _transactionId = transactionId;
        }

        public static SqlQueryCommand Create(IServiceClient client, Uri endpoint,
                                             ExecutionContext context,
                                             String selectExpression, String transactionId)
        {
            if (string.IsNullOrEmpty(selectExpression))
                throw new ArgumentException(
                    Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty);

            return new SqlQueryCommand(client, endpoint, context,
                                       selectExpression, transactionId);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            var selectExpression = _selectExpression;
            if (!selectExpression.EndsWith(";", StringComparison.OrdinalIgnoreCase))
            {
                selectExpression += ";";
            }
            parameters.Add("SelectExpression", selectExpression);
            if (!string.IsNullOrEmpty(_transactionId))
            {
                parameters.Add("TransactionID", _transactionId);
            }
        }
    }
}
