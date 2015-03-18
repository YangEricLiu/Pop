using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum ConfigChangedTarget
    {
        Origin = 1,
        EnergyConsumption = 2,
        Carbon = 3,
        Cost = 4,
        Ratio = 5,
        
        CarbonFactor = 6,
        Benchmark = 7,
        Labelling = 8
    }
}
