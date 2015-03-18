/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
    public abstract class OpenTableServiceResult
    {
        [XmlElement("RequestID")]
        public string RequestId { get; set; }

        [XmlElement("HostID")]
        public string HostId { get; set; }

        protected OpenTableServiceResult()
        {
        }
    }
}
