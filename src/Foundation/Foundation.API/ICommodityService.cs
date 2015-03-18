using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System;
using System.ServiceModel;
namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface ICommodityService
    {
        [OperationContract]
        CommodityDto[] GetAllCommodity(bool? isOther);

        [OperationContract]
        CommodityDto GetCommodityById(long commodityId);

    }
}
