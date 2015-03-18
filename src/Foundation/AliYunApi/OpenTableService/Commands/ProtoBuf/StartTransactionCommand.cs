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
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Commands.ProtoBuf
{
    /// <summary>
    /// Description of StartTransactionCommand.
    /// </summary>
    internal class StartTransactionCommand : OtsCommand<StartTransactionResponse>
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
                return HttpMethod.Post;
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
            var request = new StartTransactionRequest()
                                {
                                    entity_name = _entityName,
                                    partition_key_value = ProtoBufUtility.XmlPartitionKeyValue2ProtoBufColumnValue(this._partitionKeyValue)
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
