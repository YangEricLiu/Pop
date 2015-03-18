﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
    public class StartTransactionResult : OpenTableServiceResult
    {
        [XmlElement("TransactionID")]
        public string TransactionId { get; set; }

        public StartTransactionResult()
        {
        }
    }
}
