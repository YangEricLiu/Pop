/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;
using Aliyun.OpenServices.OpenTableService.Model;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of StartTransactionCommand.
    /// </summary>
    internal class StartTransactionCommand : OtsCommand<StartTransactionResult>
    {
        public const string ActionName = "StartTransaction";
        
        private string _entityName;
        
        private PartitionKeyValue _partitionKeyValue;

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

        protected StartTransactionCommand(IServiceClient client,
                                       Uri endpoint,
                                       ExecutionContext context,
                                       String entityName, PartitionKeyValue partitionKeyValue)
            : base(client, endpoint, context)
        {
            _entityName = entityName;
            _partitionKeyValue = partitionKeyValue;
        }

        public static StartTransactionCommand Create(IServiceClient client,
                                       Uri endpoint,
                                       ExecutionContext context,
                                       String entityName, PartitionKeyValue partitionKeyValue)
        {
            if (string.IsNullOrEmpty(entityName) || !OtsUtility.IsEntityNameValid(entityName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "entityName");
            return new StartTransactionCommand(client, endpoint, context, entityName, partitionKeyValue);
        }
        
        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("EntityName", _entityName);
            parameters.Add("PartitionKeyValue", _partitionKeyValue.ToParameterString());
            parameters.Add("PartitionKeyType", PartitionKeyTypeHelper.GetString(_partitionKeyValue.ValueType));
        }
    }
}
