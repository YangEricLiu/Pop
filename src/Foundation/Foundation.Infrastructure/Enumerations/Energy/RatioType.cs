using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Enumerations.Energy
{
    public enum RatioType
    {
        DayNight = 1,
        WorkDay = 2,
        DataValue = 3,

        //only use in DA api
        TargetLineDayNight = 4,
        BaseLineDayNight = 5,
        TargetLineWorkDay = 6,
        BaseLineWorkDay = 7,
    }
}
