using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    /// <summary>
    /// Enumeration of emission conversion factor types
    /// </summary>
    public enum CarbonFactorType
    {
        /// <summary>
        /// Emission conversion factor from measurement comodities to standard coal
        /// </summary>
        CommodityToStandardCoal = 1,

        /// <summary>
        /// Emission conversion factor from standard coal to CO2
        /// </summary>
        StandardCoalToCO2 = 2,

        /// <summary>
        /// Emission conversion factor from CO2 to tree
        /// </summary>
        CO2ToTree = 3
    }
}
