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
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Commands.ProtoBuf
{
    /// <summary>
    /// Description of DeleteTableCommand.
    /// </summary>
    internal class DeleteTableCommand : OtsCommand
    {
        private string _tableName;

        public const string ActionName = "DeleteTable";

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

        protected DeleteTableCommand(IServiceClient client,
                                     Uri endpoint,
                                     ExecutionContext context,
                                     String tableName)
            : base(client, endpoint, context)
        {
            Debug.Assert(!string.IsNullOrEmpty(tableName) &&
                         OtsUtility.IsEntityNameValid(tableName));

            _tableName = tableName;
        }

        public static DeleteTableCommand Create(IServiceClient client,
                                                Uri endpoint,
                                                ExecutionContext context,
                                                String tableName)
        {
            if (string.IsNullOrEmpty(tableName) ||
                !OtsUtility.IsEntityNameValid(tableName))
                throw new ArgumentException(OtsExceptions.NameFormatIsInvalid, "tableName");

            return new DeleteTableCommand(client, endpoint, context, tableName);
        }

        protected override void AddRequestParameters(IDictionary<string, string> parameters)
        {
            var request = new DeleteTableRequest()
                                {
                                    table_name = this._tableName
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