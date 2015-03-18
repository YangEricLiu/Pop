/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: EnergyErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error code for energy module
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum EnergyErrorCode
    {
        EnergyDataIsNull = 001,
        EnergyDataUtcTimeIsNull = 002,
        EnergyDataLocalTimeIsNull = 003,
        AggregationStepIsIllegal = 004,
        TagIdNotSame = 005,
        TagsAreNull = 006,
        StartTimeEqualOrLaterThanEndTime = 007,
        VTagTypeIsNotMeasurement = 008,
        PTagHasNoAggregation = 009,
        VTagHasNoDimension = 010,
        TagTypeError = 011,
        TimeSpansAreNull = 012,
        TagDoesNotExist = 013,
        MaxRetrieveRowCountIsIllegal = 014,
        MaxUpdateRowCountIsIllegal = 015,
        AggregateCannotBetweenKPIAndOtherTags = 016,
        //TagAssociationChanged = 017,
        TagAssociationIncorrect = 018,
        TimeRangeShouldBeOne = 019,
        EmptySvgContentOrTitle = 020,

        ConstructExcelFail = 021,
        ParseSvgFailed = 022,
        AggregateCannotBetweenDifferentCommodityTags = 023,

        ChartConvertFailed = 024,
        AggregateBetweenIllegalCommodity = 025,
        AggregateParameterIllegal = 026,

        VEEStepIllegal = 027,

        //Uom & carbon conversion error codes
        //CanNotConvertEmptyTagArray = 101,
        //CanNotConvertDifferentCommodityToCommonUom = 102,
        //CanNotConvertDifferentUomGroupToCommonUom = 103,
        CanNotConvertGroupNotEnergyConsumption = 104,
        CanNotConvertUomNotMatchCommodity = 105,
        CanNotConvertUomGroupHasNoUom = 106,
        CanNotConvertNoCommonOrBaseUomInGroup = 107,
        CanNotConvertUomNotInSameGroup = 108,
        CanNotConvertCommodityNotSame = 109,
        //CanNotConvertNullTagInArray = 110,
        //CanNotConvertEmptyTagEnergyDataArray = 110,
        //CanNotConvertNoTagEnergyDataInArray = 111,
        //CanNotConvertTagNotMeasurement = 112,
        //CanNotConvertIllegalEntityInTagArray = 113,
        CanNotConvertNeitherCommonUomNorStandardCoal = 114,

        //KPI
        //KPIHasNoTargetBaseline = 201,
        //KPIHasNoEnergyData = 202,
        KPIVTagDoesNotExist = 203,
        //KPIDataDoesNotExist = 204,
        DayNightRatioKPIDoesNotSupportHourly = 205,

        //Carbon usage
        HierarchyDoesNotExist = 301,
        HierarchyNodeHasMoreOrLessThanOneTotalConsumptionVTag = 302,
        //CommodityVTagDoesNotBelongToHierarchyNode = 303,
        //HierarchyNodeHasNoCommodityEnergyConsumptionVTag = 304,
        CanNotConvertTagUnidentifiable = 305,
        CurrentHierarchyNodeHasNoConsumptionTag = 307,

        /// <summary>
        /// Used in CostDataBL
        /// </summary>
        GetCommodityParamsError = 306,


        //Cost
        WrongAssociatedId = 401,
        AggregationIsEmpty = 402,
        TimeRangeIsNull = 403,
        StartTimeIsIllegal = 404,
        EndTimeIsIllegal = 405,
        EnergyConsumptionTagIsNull = 406,
        TouNotSupportHourlyRaw = 407,
        TouNotConfiged = 408,


        //Unit/labeling
        InvalidParameter = 500,
        TotalPersonHaveNotBeenSet = 501,
        TotalAreaHaveNotBeenSet = 502,
        HeatingAreaHaveNotBeenSet = 503,
        CoolingAreaHaveNotBeenSet = 504,

        TotalPersonHaveNotBeenSet4TimeRange = 505,
        TotalAreaHaveNotBeenSet4TimeRange = 506,
        HeatingAreaHaveNotBeenSet4TimeRange = 507,
        CoolingAreaHaveNotBeenSet4TimeRange = 508,
        LabellingDeleted=509,
        LabellingNoData = 510,

        //PersonAreaHaveNotBeenSet = 501,
        //PersonAreaHaveNotBeenSetForTimeRange = 502,


        //Ratio/labeling
        TagHierarchyHasNoDayNightProperty=601,
        TagHierarchyHasNoWorkDayProperty = 602,

        //Ranking
        SomHierarchiesAreDeleted = 701,
        MissingConfig = 702,

        //open
        NotSupportCalcValue = 801,
        OnlySupportOrigValue = 802,
        InvalidTagsParameter = 803,
        InvalidTimeRangesParameter = 804,
        InvalidCustomerCodeParameter = 805,
        InvalidValueOptionsParameter = 806,
        OnlySupportCalcValue = 807,
        InvalidDataOptionsParameter = 808
    }
}