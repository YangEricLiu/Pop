/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.OpenTableService.Model
{
    public class ListTableResult : OpenTableServiceResult
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), XmlArray]
        [XmlArrayItem(ElementName="TableName")]
        public List<string> TableNames { get; set; }

        public ListTableResult()
        {
        }
    }
}
