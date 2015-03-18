/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
    [XmlRoot("Error")]
    

    public class ErrorResult : OpenTableServiceResult
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
