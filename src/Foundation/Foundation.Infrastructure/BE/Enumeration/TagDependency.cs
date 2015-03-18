namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    using System;

    /// <summary>
    /// TagDependency
    /// </summary>
    [Flags]
    public enum TagDependency
    {
        /// <summary>
        /// Hierarchy
        /// </summary>
        Hierarchy = 1,
        /// <summary>
        /// SystemDimension
        /// </summary>
        SystemDimension = 2,
        /// <summary>
        /// AreaDimension
        /// </summary>
        AreaDimension = 4,


        SystemAndArea=6
    }
}