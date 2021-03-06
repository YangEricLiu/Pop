﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Utilities;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of DeleteTableGroupCommand.
    /// </summary>
    internal class DeleteTableGroupCommand : OtsCommand
    {
        private string _tableGroupName;

        public const string ActionName = "DeleteTableGroup";

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

        protected DeleteTableGroupCommand(IServiceClient client,
                                       Uri endpoint,
                                       ExecutionContext context,
                                       String tableGroupName)
            : base(client, endpoint, context)
        {
            Debug.Assert(!string.IsNullOrEmpty(tableGroupName) &&
                         OtsUtility.IsEntityNameValid(tableGroupName));

            _tableGroupName = tableGroupName;
        }

        public static DeleteTableGroupCommand Create(IServiceClient client,
                                                     Uri endpoint,
                                                     ExecutionContext context,
                                                     String tableGroupName)
        {
            if (string.IsNullOrEmpty(tableGroupName) ||
                !OtsUtility.IsEntityNameValid(tableGroupName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableGroupName");

            return new DeleteTableGroupCommand(client, endpoint, context, tableGroupName);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableGroupName", _tableGroupName);
        }
    }

}
