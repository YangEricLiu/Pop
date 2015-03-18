/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Transform;
using System.IO;

namespace Aliyun.OpenServices.OpenTableService.Utilities
{
    /// <summary>
    /// The factory to create deserialization instances.
    /// </summary>
    internal class DeserializerFactory
    {
        public DeserializerFactory()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IDeserializer<Stream, T> CreateDeserializer<T>(string contentType)
        {
            if (contentType.Contains("protocol"))
            {
                return new ProtoBufStreamDeserializer<T>();
            }
            else //if (contentType.Contains("xml"))
            {
                return new XmlStreamDeserializer<T>();
            }
        }
    }
}