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

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of CreateTableGroupCommand.
    /// </summary>
    internal class CreateTableGroupCommand : OtsCommand
    {
        private string _tableGroupName;
        private PartitionKeyType _partitionKeyType;

        public const string ActionName = "CreateTableGroup";

        protected override HttpMethod Method
        {
            get
            {
                return HttpMethod.Get;
            }
        }

        protected override string ResourcePath
        {
            get
            {
                return ActionName;
            }
        }

        protected CreateTableGroupCommand(IServiceClient client,
                                          Uri endpoint,
                                          ExecutionContext context,
                                          String tableGroupName,
                                          PartitionKeyType partitionKeyType)
            : base(client, endpoint, context)
        {
            if (string.IsNullOrEmpty(tableGroupName) ||
                !OtsUtility.IsEntityNameValid(tableGroupName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableGroupName");

            _tableGroupName = tableGroupName;
            _partitionKeyType = partitionKeyType;
        }
        
        public static CreateTableGroupCommand Create(IServiceClient client,
                                                Uri endpoint,
                                                ExecutionContext context,
                                                String tableGroupName,
                                                PartitionKeyType partitionKeyType)
        {
            return new CreateTableGroupCommand(client, endpoint, context, tableGroupName, partitionKeyType);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableGroupName", _tableGroupName);
            parameters.Add("PartitionKeyType", PartitionKeyTypeHelper.GetString(_partitionKeyType));
        }
    }
}
