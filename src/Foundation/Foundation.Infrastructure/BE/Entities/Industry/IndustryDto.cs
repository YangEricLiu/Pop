/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: CommodityDto.cs
 * Author	    : Aires Zhang
 * Date Created : 2012-05-10
 * Description  : Commodity data transfer object
--------------------------------------------------------------------------------------------*/


using SE.DSP.Foundation.Infrastructure.BaseClass;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class IndustryDto : DtoBase
    {
        public string Code { get; set; }

        public long? ParentId { get; set; }

        public string Comment { get; set; }
    }
}