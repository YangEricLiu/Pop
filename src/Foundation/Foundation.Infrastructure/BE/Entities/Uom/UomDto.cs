/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: UomDto.cs
 * Author	    : Aires Zhang
 * Date Created : 2012-05-10
 * Description  : Uom data transfer object
--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BaseClass;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Runtime.Serialization;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    [Serializable]
    [DataContract]
    public class UomDto : DtoBase
    {
        /// <summary>
        /// uom code
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// uom comment
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        /// uom precision
        /// </summary>
        [DataMember]
        public int Precision { get; set; }
    }
}
