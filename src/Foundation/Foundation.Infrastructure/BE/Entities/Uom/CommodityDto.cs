/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: CommodityDto.cs
 * Author	    : Aires Zhang
 * Date Created : 2012-05-10
 * Description  : Commodity data transfer object
--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using System.Runtime.Serialization;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
      [Serializable]
      [DataContract]
    public class CommodityDto : DtoBase
    {
        /// <summary>
        /// Commodity code
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        
        /// <summary>
        /// Commodity comment
        /// </summary>
        [DataMember]
          public string Comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
          public UomDto[] Uoms { get; set; }
    }
}
