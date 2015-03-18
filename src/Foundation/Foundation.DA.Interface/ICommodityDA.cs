
using SE.DSP.Foundation.Infrastructure.BE.Entities;

namespace SE.DSP.Foundation.DA.Interface
{
    public interface ICommodityDA
    {
        CommodityEntity RetrieveCommodityById(long commodityId);
        CommodityEntity[] RetrieveAllCommodity();
    }
}