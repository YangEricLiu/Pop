
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.DA.Interface
{
    public interface IUomDA
    {
        UomEntity RetrieveUomById(long uomId);
        UomEntity[] RetrieveAllUom();
        UomEntity[] RetrieveUomByCommodityId(long commodityId);
    }
}