
namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum ConfigChangedSource
    {
        TagCreation = 001,
        TagModification = 002,
        TagDeletation = 003,
        AreaDimensionDeletation = 004,
        SystemDimensionDeletation = 005,
        HierarchyDeletation = 006,
        CustomerDeletation = 007,
        AdvancedPropertyChanged = 008,
        CostSettingTagChanged = 009,
        TouSettingChanged = 010,
        CalendarChanged=011,
        TagAssociated=012,
        TagAssociateCancel=013,
        TagEnergyConsumption=014,
        TagEnergyConsumptionCancel=015,
        HierarchyCalendarChanged=016,
        BenchmarkCalcuationStatus=017,
        HierarchyMoved=018
    }
}
