/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Aliyun.OpenServices.Common.Communication;
using ProtoBuf;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// Description of ProboBufResponseDeserializer.
    /// </summary>
    internal class ProtoBufStreamDeserializer<T> : IDeserializer<Stream, T>
    {
        public ProtoBufStreamDeserializer()
        {
        }

        /// <summary>
        /// Deserialize the result to an object of T from the <see cref="ServiceResponse" />.
        /// It will close the underlying stream.
        /// </summary>
        /// <param name="pb"></param>
        /// <returns></returns>
        public T Deserialize(Stream pb)
        {
            Debug.Assert(pb != null);
            using (pb)
            {
                try
                {
                    return Serializer.Deserialize<T>(pb);
                }
                catch (XmlException ex)
                {
                    throw new ResponseDeserializationException(ex.Message, ex);
                }
                catch (InvalidOperationException ex)
                {
                    throw new ResponseDeserializationException(ex.Message, ex);
                }
            }
        }
    }
}