/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using System.IO;

namespace Aliyun.OpenServices.Common.Utilities
{
    /// <summary>
    /// Description of IOUtils.
    /// </summary>
    internal static class IOUtils
    {
        private const int _bufferSize = 1024 * 4;
        
        public static void WriteTo(this Stream src, Stream dest){
            if (dest == null)
                throw new ArgumentNullException("dest");
            
            byte[] buffer = new byte[_bufferSize];
            int bytesRead = 0;
            while((bytesRead = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, bytesRead);
            }
            dest.Flush();
        }
    }
}
