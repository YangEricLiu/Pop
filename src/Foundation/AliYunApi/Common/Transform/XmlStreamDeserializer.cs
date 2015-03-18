/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Aliyun.OpenServices.Common.Communication;

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// Description of XmlResponseDeserializer.
    /// </summary>
    internal class XmlStreamDeserializer<T> : IDeserializer<Stream, T>
    {
        public XmlStreamDeserializer()
        {
        }
        
        /// <summary>
        /// Deserialize the result to an object of T from the <see cref="ServiceResponse" />.
        /// It will close the underlying stream.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T Deserialize(Stream xml)
        {
            Debug.Assert(xml != null);
            using (xml)
            {
                // TODO: Cache the serializer to improve perf.
                try
                {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(xml);
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
