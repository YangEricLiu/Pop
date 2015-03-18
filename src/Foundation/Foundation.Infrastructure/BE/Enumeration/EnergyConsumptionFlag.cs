namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    [System.Flags]
    public enum EnergyConsumptionFlag
    {
        None = 0,
        Hierarchy= 1,
        SystemDimension =2,
        AreaDimension=4,
        Dimension = SystemDimension | AreaDimension,
    }
}
