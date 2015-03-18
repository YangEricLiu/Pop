using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities.DataService
{
    [DataServiceQueryRoute("/{commodityId}-{industryId}-{zoneId}/{step}/{startTime}~{endTime}")]
    public class CostBenchmarkDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }

        [DataServiceQueryRouteParameter]
        public long IndustryId { get; set; }

        [DataServiceQueryRouteParameter]
        public long ZoneId { get; set; }

        [DataServiceQueryRouteParameter]
        public TimeGranularity Step { get; set; }

        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }

        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{hierarchyId}-{systemDimensionItemId}-{areaDimensionId}-{commodityId}/{step}/{startTime}~{endTime}")]
    public class CostDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long HierarchyId { get; set; }
        [DataServiceQueryRouteParameter]
        public long? SystemDimensionItemId { get; set; }
        [DataServiceQueryRouteParameter]
        public long? AreaDimensionId { get; set; }
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{hierarchyId}-{areaDimensionId}-{systemDimensionItemId}-{commodityId}-{targetBaselineCode}/{step}/{startTime}~{endTime}")]
    public class CostTargetBaselineDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long? HierarchyId { get; set; }
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public string TargetBaselineCode { get; set; }
        [DataServiceQueryRouteParameter]
        public long? SystemDimensionItemId { get; set; }
        [DataServiceQueryRouteParameter]
        public long? AreaDimensionId { get; set; }
        [DataServiceQueryRouteParameter]

        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{tagId}/{startTime}~{endTime}")]
    public class DemandDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TagId { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{commodityId}-{industryId}-{zoneId}/{step}/{startTime}~{endTime}")]
    public class EnergyConsumptionBenchmarkDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public long IndustryId { get; set; }
        [DataServiceQueryRouteParameter]
        public long ZoneId { get; set; }
        [DataServiceQueryRouteParameter]

        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{hierarchyId}-{systemDimensionItemId}-{commodityId}/{step}/{startTime}~{endTime}")]
    public class EnergyConsumptionDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long HierarchyId { get; set; }
        [DataServiceQueryRouteParameter]
        public long? SystemDimensionItemId { get; set; }
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]

        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{spId}-{labellingType}/{commodityId}-{industryId}-{zoneId}/{month}")]
    public class LabellingDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long SpId { get; set; }
        [DataServiceQueryRouteParameter]

        public int LabellingType { get; set; }
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public long IndustryId { get; set; }
        [DataServiceQueryRouteParameter]
        public long ZoneId { get; set; }
        [DataServiceQueryRouteParameter]
        public long Month { get; set; }
    }
    [DataServiceQueryRoute("/{realTagId}-{relativeTagId}/{startTime}~{endTime}")]
    public class PowerFactorDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long RealTagId { get; set; }
        [DataServiceQueryRouteParameter]
        public long RelativeTagId { get; set; }
        [DataServiceQueryRouteParameter]

        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{tagId}/{startTime}~{endTime}")]
    public class RawDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TagId { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{commodityId}-{industryId}-{zoneId}/{step}/{startTime}~{endTime}")]
    public class StandardCoalBenchmarkDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public long IndustryId { get; set; }
        [DataServiceQueryRouteParameter]
        public long ZoneId { get; set; }
        [DataServiceQueryRouteParameter]

        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{hierarchyId}-{commodityId}/{step}/{startTime}~{endTime}")]
    public class StandardCoalDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long HierarchyId { get; set; }
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{hierarchyId}-{commodityId}-{targetBaselineCode}/{step}/{startTime}~{endTime}")]
    public class StandardCoalTargetBaselineDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long HierarchyId { get; set; }
        [DataServiceQueryRouteParameter]
        public long CommodityId { get; set; }
        [DataServiceQueryRouteParameter]
        public string TargetBaselineCode { get; set; }
        [DataServiceQueryRouteParameter]

        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{tagId}-{anomalyType}/{startTime}~{endTime}")]
    public class TagAnomalyDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TagId { get; set; }
        [DataServiceQueryRouteParameter]
        public int AnomalyType { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("{tagId}-{userId}/{modifyTime}/{recordStartTime}~{recordEndTime}")]
    public class TagDataModifyLogDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TagId { get; set; }
        [DataServiceQueryRouteParameter]

        public long UserId { get; set; }
        [DataServiceQueryRouteParameter]
        public long? ModifyTime { get; set; }
        [DataServiceQueryRouteParameter]

        public long? RecordStartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long? RecordEndTime { get; set; }
    }
    [DataServiceQueryRoute("/{tagId}/{step}/{startTime}~{endTime}")]
    public class TagDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TagId { get; set; }

        [DataServiceQueryRouteParameter]
        public TimeGranularity Step { get; set; }

        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }

        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{targetBaselineId}/{step}/{startTime}~{endTime}")]
    public class TargetBaselineDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TargetBaselineId { get; set; }
        [DataServiceQueryRouteParameter]
        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }
    [DataServiceQueryRoute("/{targetBaselineId}/{step}/{startTime}~{endTime}")]
    public class TargetBaselineUnitDataQuery : DataServiceQueryBase
    {
        [DataServiceQueryRouteParameter]
        public long TargetBaselineId { get; set; }
        [DataServiceQueryRouteParameter]
        public TimeGranularity Step { get; set; }
        [DataServiceQueryRouteParameter]
        public long StartTime { get; set; }
        [DataServiceQueryRouteParameter]
        public long EndTime { get; set; }
    }  
}
