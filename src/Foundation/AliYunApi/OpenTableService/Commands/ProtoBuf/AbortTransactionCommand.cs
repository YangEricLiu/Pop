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
    /// Description of AbortTransactionCommand.
    /// </summary>
    internal class AbortTransactionCommand : OtsCommand
    {
        public const string ActionName = "AbortTransaction";

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
                return HttpMethod.Post;
            }
        }

        protected AbortTransactionCommand(IServiceClient client,
                                          Uri endpoint,
                                          ExecutionContext context,
                                          String transactionId)
            : base(client, endpoint, context)
        {
            if (string.IsNullOrEmpty(transactionId))
                throw new ArgumentException(
                    Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty,
                    "transactionId");

            _transactionId = transactionId;
        }

        public static AbortTransactionCommand Create(IServiceClient client,
                                                     Uri endpoint,
                                                     ExecutionContext context,
                                                     String transactionId)
        {
            return new AbortTransactionCommand(client, endpoint, context, transactionId);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            var request = new AbortTransactionRequest()
            {
                transaction_id = this._transactionId
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
