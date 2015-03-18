using SE.DSP.Foundation.Infrastructure.BE.Entities;
using System;
using System.ServiceModel;
namespace SE.DSP.Foundation.API
{
    [ServiceContract]
    public interface IUomService
    {
        [OperationContract]
        UomDto[] GetAllUom();

        [OperationContract]
        UomGroupRelationEntity GetCommodityUom(long commodityId, long uomId);

        [OperationContract]
        UomDto[] GetUomByCommodityId(long commodityId);

        [OperationContract]
        UomGroupRelationEntity[] GetUomByGroup(long groupId);

        [OperationContract]
        UomDto GetUomById(long uomId);

        [OperationContract]
        UomGroupEntity GetUomGroupByCommodityAndUom(long commodityId, long uomId);

        [OperationContract]
        UomGroupEntity GetEnergyConsumptionGroupByCommodity(long commodityId);

        [OperationContract]
        UomGroupRelationEntity GetCommonUomInGroup(long groupId);

        [OperationContract]
        long GetCommonUomId(long commodityId, long uomId);

        [OperationContract]
        UomGroupRelationEntity GetCommonUom(long commodityId, long uomId);

        [OperationContract]
        UomConversionDto[] GetCommonConversionFactors();

        [OperationContract]
        decimal? GetUomFactorToDestinationUom(long commodityId, long sourceUomId, long destinationUomId);
    }
}
