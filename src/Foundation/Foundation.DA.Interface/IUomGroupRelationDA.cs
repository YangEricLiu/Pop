using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.DA.Interface
{
    public interface IUomGroupRelationDA
    {
        UomGroupRelationEntity RetrieveCommodityUom(long commodityId, long uomId);
        UomGroupRelationEntity[] RetrieveUomByGroup(long groupId);
        UomGroupRelationEntity[] RetrieveCarbonConvertableCommodity();
        UomGroupRelationEntity[] RetrieveUomRelationByCommodityId(long commodityId);
        UomGroupRelationEntity[] RetrieveUomRelation();
    }
}
