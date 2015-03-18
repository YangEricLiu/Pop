using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities.DataService
{
    public class CostBenchmarkDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long CommodityId { get; set; }
        public long IndustryId { get; set; }
        public long ZoneId { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }
    }
    public class CostDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long HierarchyId { get; set; }
        public long CommodityId { get; set; }
        public long? SystemDimensionItemId { get; set; }
        public long? AreaDimensionId { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? Value { get; set; }
        public long Quality { get; set; }

        public double? TouPeak { get; set; }
        public double? TouPlain { get; set; }
        public double? TouValley { get; set; }


        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }

        public long? RankingValue { get; set; }
        public long? TotalCount { get; set; }
    }
    public class CostTargetBaselineDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long HierarchyId { get; set; }
        public long CommodityId { get; set; }
        public string TargetBaselineCode { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? Value { get; set; }
        public long Quality { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }


    }
    public class DemandDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TagId { get; set; }
        public long Time { get; set; }
        public double? Value { get; set; }
    }
    public class EnergyConsumptionBenchmarkDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long CommodityId { get; set; }
        public long IndustryId { get; set; }
        public long ZoneId { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }

        public double? DayNightRatio { get; set; }
        public double? WorkDayRatio { get; set; }
    }
    public class EnergyConsumptionDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long HierarchyId { get; set; }
        public long? SystemDimensionItemId { get; set; }
        public long CommodityId { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? Value { get; set; }
        public long Quality { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }

        public long? RankingValue { get; set; }
        public long? TotalCount { get; set; }

    }
    public class LabellingDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {

        public long SpId { get; set; }

        public int LabellingType { get; set; }
        public long CommodityId { get; set; }
        public long IndustryId { get; set; }
        public long ZoneId { get; set; }
        public int Month { get; set; }
        public double? AverageValue { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
    }
    public class PowerFactorDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long RealTagId { get; set; }
        public long RelativeTagId { get; set; }
        public long Time { get; set; }
        public double? Value { get; set; }
    }
    public class RawDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TagId { get; set; }
        public long Time { get; set; }
        public double? Value { get; set; }
        public long Quality { get; set; }
        public double? RawValue { get; set; }
    }
    public class StandardCoalBenchmarkDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long CommodityId { get; set; }
        public long IndustryId { get; set; }
        public long ZoneId { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }
    }
    public class StandardCoalDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long HierarchyId { get; set; }
        public long CommodityId { get; set; }
        public TimeGranularity Step { get; set; }
        public long Time { get; set; }
        public double? Value { get; set; }
        public long Quality { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }
        public double? RankingValue { get; set; }
        public double? TotalCount { get; set; }
    }
    public class StandardCoalTargetBaselineDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long HierarchyId { get; set; }
        public long CommodityId { get; set; }
        public string TargetBaselineCode { get; set; }

        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? Value { get; set; }
        public long Quality { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }
    }
    public class TagAnomalyDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TagId { get; set; }
        public int AnomalyType { get; set; }
        public long Time { get; set; }

        public double? Value { get; set; }

        public long RuleId { get; set; }
        public int Status { get; set; }
    }
    public class TagDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TagId { get; set; }
        public TimeGranularity Step { get; set; }
        public long Time { get; set; }
        public double? Value { get; set; }
        public long Quality { get; set; }

        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }

        public double? DayNightRatio { get; set; }
        public double? WorkDayRatio { get; set; }
    }
    public class TagDataModifyLogEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TagId { get; set; }

        public long UserId { get; set; }
        public long ModifyTime { get; set; }
        public long RecordTime { get; set; }

        public double? Original { get; set; }
        public double? New { get; set; }
        public string UserName { get; set; }
    }
    public class TargetBaselineDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TargetBaselineId { get; set; }
        public TimeGranularity Step { get; set; }
        public long Time { get; set; }
        public double? Value { get; set; }
        public bool? IsModify { get; set; }
    }
    public class TargetBaselineUnitDataEntity : SE.DSP.Foundation.Infrastructure.Utils.IDataServiceEntity
    {
        public long TargetBaselineId { get; set; }
        public TimeGranularity Step { get; set; }
        public long Time { get; set; }

        public double? Value { get; set; }
        public double? TotalPerson { get; set; }
        public double? TotalArea { get; set; }
        public double? CoolingArea { get; set; }
        public double? HeatingArea { get; set; }
        public double? DayNightRatio { get; set; }
        public double? WorkDayRatio { get; set; }
    }
}
