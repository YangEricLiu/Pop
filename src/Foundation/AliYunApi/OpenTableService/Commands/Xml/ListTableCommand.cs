/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.Model;

namespace Aliyun.OpenServices.OpenTableService.Commands
{
    /// <summary>
    /// Description of ListTablesCommand.
    /// </summary>
    internal class ListTableCommand : OtsCommand<ListTableResult>
    {
        public const string ActionName = "ListTable";

        protected override string ResourcePath
        {
            get { return ActionName; }
        }

        protected override HttpMethod Method
        {
            get { return HttpMethod.Get; }
        }

        protected ListTableCommand(IServiceClient client,
                                      Uri endpoint,
                                      ExecutionContext context)
            : base(client, endpoint, context)
        {
        }

        public static ListTableCommand Create(IServiceClient client,
                                      Uri endpoint,
                                      ExecutionContext context)
        {
            return new ListTableCommand(client, endpoint, context);
        }
    }
}
