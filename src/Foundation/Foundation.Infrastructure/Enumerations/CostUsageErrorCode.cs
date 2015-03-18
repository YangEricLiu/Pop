/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: CostUsageErrorCode.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Error code for cost usage module
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    /// <summary>
    /// Cost usage error code
    /// </summary>
    public enum CostUsageErrorCode
    {
        WrongAssociatedId = 001,
        AggregationIsEmpty = 002,
        TimeRangeIsNull = 003,
        StartTimeIsIllegal = 004,
        EndTimeIsIllegal = 005,
        EnergyConsumptionTagIsNull = 006,
        TouNotSupportHourly = 007
    }
}
