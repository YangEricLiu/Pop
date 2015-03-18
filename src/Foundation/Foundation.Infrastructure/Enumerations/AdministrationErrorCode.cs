/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: AdministrationErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error code for administration module
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// The detail error code of administration module, each has 3 numbers.
    /// </summary>
    public enum AdministrationErrorCode
    {
        //carbon factor and uom, from 001 to 024
        CommodityUomNotExist = 004,
        DuplicatedCarbonFactor = 005,
        CarbonFactorNotExit = 006,
        CarbonFactorChanged = 007,
        FactorTypeCommodityNotMatch = 008,


        //tou tariff from 025 to 050
        TouTariffChanged = 025,
        PeakTariffChanged = 026,
        TouTariffIsNull = 027,
        TouTariffDoesNotExist = 028,
        PeakTariffDoesNotExist = 029,
        TouTimeRangeEmpty = 030,
        TouTimeRangeError = 032,
        TouTimeRangeMustContainPeakAndValleyItemsWhenNoPlainItem = 033,
        PeakTariffTimeRangeEmpty = 034,
        PeakTariffTimeOverlap = 035,
        InputPeakTariffNull = 036,
        NotAllowToDeleteOrModifyReferencedData = 038,
        PeakTariffTimeEmpty = 039,
        TouTariffNameReduplicate = 040,
        PeakTariffAlreadyExist = 041,
        TariffPriceCanNotBeZero = 042,

        //calendar from 051 to 100
        CalendarItemsMissing = 051,
        CalendarStartDateMoreThanEndDate = 052,
        CalendarTimeOverlap = 053,
        CalendarNameReduplicate = 054,
        CalendarHasBeenDeleted = 055,
        CalendarHasBeenModified = 056,
        CalendarStartTimeEqualOrMoreThanEndTime = 057,
        CalendarHasBeenUsed = 058,
        FebruaryDayIllegal = 059,
        SmallMonthDayIllegal = 060,
        ColdWarmItemsMissing = 061,
        ColdWarmItemInSameMonth = 062,
        ColdWarmItemIntervalLessThan7Days = 063,
        HierarchyHasBeenDeleted = 064,

        BenchmarkExists = 070,
        BenchmarkHasBeenDeleted = 071,
        BenchmarkHasBeenModified = 072,
        BenchmarkHasNoZone = 073,

        LabelingExists = 080,
        LabelingHasBeenDeleted = 081,
        LabelingHasBeenModified = 082,
        LabelingHasNoZone = 083,
        LabelingGradeIllegal = 084,
        LabelingYearIllegal = 085
    }
}