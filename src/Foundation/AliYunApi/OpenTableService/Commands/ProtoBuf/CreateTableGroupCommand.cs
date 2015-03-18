/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.OpenTableService.ProtoBuf;
using Aliyun.OpenServices.OpenTableService.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Commands.ProtoBuf
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
                return HttpMethod.Post;
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
            var request = new CreateTableGroupRequest()
            {
                table_group_name = this._tableGroupName,
                partition_key_type = ProtoBufUtility.XmlPartitionKeyType2ProtoBufColumnType(this._partitionKeyType)
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