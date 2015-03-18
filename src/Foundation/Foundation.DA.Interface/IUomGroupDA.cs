using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.DA.Interface
{
    public interface IUomGroupDA
    {
        UomGroupEntity RetrieveUomGroupById(long uomGroupId);

        UomGroupEntity[] RetrieveUomGroupByCommodity(long commodityId);

        UomGroupEntity RetrieveUomGroupByCommodityAndUom(long commodityId, long uomId);
    }
}
