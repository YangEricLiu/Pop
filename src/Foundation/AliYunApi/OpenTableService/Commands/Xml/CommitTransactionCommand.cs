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
    /// <summary>
    /// Description of CommitTransactionCommand.
    /// </summary>
    internal class CommitTransactionCommand : OtsCommand
    {
        public const string ActionName = "CommitTransaction";
        
        private string _transactionId;
        
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
                return HttpMethod.Get;
            }
        }
        
        protected CommitTransactionCommand(IServiceClient client,
                                           Uri endpoint,
                                           ExecutionContext context,
                                           String transactionId)
            : base(client, endpoint, context)
        {
            if (string.IsNullOrEmpty(transactionId))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "transactionId");

            _transactionId = transactionId;
        }
        
        public static CommitTransactionCommand Create(IServiceClient client,
                                                      Uri endpoint,
                                                      ExecutionContext context,
                                                      String transactionId)
        {
            return new CommitTransactionCommand(client, endpoint, context, transactionId);
        }
        
        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TransactionID", _transactionId);
        }
    }
}
