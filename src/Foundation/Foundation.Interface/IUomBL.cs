using Schneider.REM.BL.Administration.DataContract;
using Schneider.REM.BL.Common;
using Schneider.REM.DA.Administration.Entity;
using System;
namespace Schneider.REM.BL.Administration.Interface
{
    public interface IUomBL
    {
        Schneider.REM.BL.Administration.DataContract.CommodityDto[] GetAllCommodity(bool? isOther);
        UomGroupRelationEntity[] GetCarbonConvertableCommodities();
        Schneider.REM.BL.Administration.DataContract.UomDto[] GetAllUom();
        ConversionDestination? GetConversionDestinationType(long[] tagIds);

        Schneider.REM.BL.Administration.DataContract.CommodityDto GetCommodityById(long commodityId);
        Schneider.REM.DA.Administration.Entity.UomGroupRelationEntity GetCommodityUom(long commodityId, long uomId);

        Schneider.REM.BL.Administration.DataContract.ConversionFactorDto GetConversionInformation();

        Schneider.REM.BL.Administration.DataContract.UomDto[] GetUomByCommodityId(long commodityId);
        Schneider.REM.DA.Administration.Entity.UomGroupRelationEntity[] GetUomByGroup(long groupId);
        Schneider.REM.BL.Administration.DataContract.UomDto GetUomById(long uomId);

        Schneider.REM.DA.Administration.Entity.UomGroupEntity GetUomGroupByCommodityAndUom(long commodityId, long uomId);
        Schneider.REM.DA.Administration.Entity.UomGroupEntity GetEnergyConsumptionGroupByCommodity(long commodityId);
        UomGroupRelationEntity GetCommonUomInGroup(long groupId);

        long GetCommonUomId(long commodityId, long uomId);
        UomGroupRelationEntity GetCommonUom(long commodityId, long uomId);

        Schneider.REM.BL.Energy.DataContract.TargetEnergyDataDto[] ConvertTargetEnergyData(Schneider.REM.BL.Energy.DataContract.TargetEnergyDataDto[] targetEnergyDataArray, Schneider.REM.BL.Common.ConversionDestination? destinationType, bool processPrecision = true);
        UomConversionDto[] GetCommonConversionFactors();
        decimal? GetUomFactorToDestinationUom(long commodityId, long sourceUomId, long destinationUomId);
        decimal? ConvertToCarbonUsage(long commodityId, long sUomId, ConversionDestination destinationType, DateTime dataTime, double? dataValue, int? precision);
        int GetEffectiveYear(long commodityId, long uomId, ConversionDestination destinationType);
    }
}
