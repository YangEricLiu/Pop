/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Aliyun.OpenServices.OpenTableService
{
    /// <summary>
    /// 该异常在对开放结构化数据服务（Open Table Service）访问失败时抛出。
    /// </summary>
    /// <seealso cref="ServiceException" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ots"), Serializable]
    public class OtsException : ServiceException
    {
        /// <summary>
        /// 初始化新的<see cref="OtsException"/>实例。
        /// </summary>
        public OtsException()
            : base()
        {
        }

        /// <summary>
        /// 初始化新的<see cref="OtsException"/>实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误信息。</param>
        public OtsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="OtsException"/>实例。
        /// </summary>
        /// <param name="info">保存序列化对象数据的对象。</param>
        /// <param name="context">有关源或目标的上下文信息。</param>
        protected OtsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 初始化新的<see cref="OtsException"/>实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误信息。</param>
        /// <param name="innerException">导致当前异常的异常。</param>
        public OtsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// 重载<see cref="ISerializable.GetObjectData"/>方法。
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/>，它存有有关所引发异常的序列化的对象数据。</param>
        /// <param name="context"><see cref="StreamingContext"/>，它包含有关源或目标的上下文信息。</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
