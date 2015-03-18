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
    /// Description of ListTableGroupsCommand.
    /// </summary>
    internal class ListTableGroupCommand : OtsCommand<ListTableGroupResult>
    {
        public const string ActionName = "ListTableGroup";

        protected override string ResourcePath
        {
            get { return ActionName; }
        }

        protected override HttpMethod Method
        {
            get { return HttpMethod.Get; }
        }

        protected ListTableGroupCommand(IServiceClient client,
                                        Uri endpoint,
                                        ExecutionContext context)
            : base(client, endpoint, context)
        {
        }

        public static ListTableGroupCommand Create(IServiceClient client,
                                                   Uri endpoint,
                                                   ExecutionContext context)
        {
            return new ListTableGroupCommand(client, endpoint, context);
        }
    }
}
