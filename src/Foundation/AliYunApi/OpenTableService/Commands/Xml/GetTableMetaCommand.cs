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
    /// Description of GetTableMetaCommand.
    /// </summary>
    internal class GetTableMetaCommand : OtsCommand<GetTableMetaResult>
    {
        private string _tableName;

        public const string ActionName = "GetTableMeta";

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

        protected GetTableMetaCommand(IServiceClient client,
                                   Uri endpoint,
                                   ExecutionContext context,
                                   String tableName)
            : base(client, endpoint, context)
        {
            _tableName = tableName;
        }

        public static GetTableMetaCommand Create(IServiceClient client,
                                   Uri endpoint,
                                   ExecutionContext context,
                                   String tableName)
        {
            if (string.IsNullOrEmpty(tableName) ||
                !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            return new GetTableMetaCommand(client, endpoint, context, tableName);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            parameters.Add("TableName", _tableName);
        }
    }

}
