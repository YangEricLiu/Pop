/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: TimeGranularity.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Time granularity
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum TimeGranularity
    {
        None = -1,
        Minite = 0,
        Hourly = 1,
        Daily = 2,
        Monthly = 3,
        Yearly = 4,
        Weekly = 5,
        Min15=6,
        Min30=7
       
    }

    public class TimeGranularityComparer : IComparer
    {
        private List<TimeGranularity> StepOrder = new List<TimeGranularity>() 
        {
            //TimeGranularity.None,
            TimeGranularity.Min15,
            TimeGranularity.Min30,
            TimeGranularity.Minite,
            TimeGranularity.Hourly,
            TimeGranularity.Daily,
            TimeGranularity.Weekly,
            TimeGranularity.Monthly,
            TimeGranularity.Yearly
        };

        public int Compare(object x, object y)
        {
            return StepOrder.IndexOf((TimeGranularity)Enum.Parse(typeof(TimeGranularity), x.ToString())) - StepOrder.IndexOf((TimeGranularity)Enum.Parse(typeof(TimeGranularity), y.ToString()));
        }

        public TimeGranularity GetMaxStep(TimeGranularity[] steps)
        {
            TimeGranularity max = TimeGranularity.None;

            for (int i = 0; i < steps.Length; i++)
            {
                if (this.Compare(max, steps[i]) < 0 && steps[i] != TimeGranularity.None)
                {
                    max = steps[i];
                }
            }

            return max;
        }

        public TimeGranularity GetMinStep(TimeGranularity[] steps)
        {
            if (steps.Length == 0)
                return TimeGranularity.Min15;
            TimeGranularity min = TimeGranularity.Yearly;

            for (int i = 0; i < steps.Length; i++)
            {
                if (this.Compare(min, steps[i]) > 0 && steps[i] != TimeGranularity.None)
                {
                    min = steps[i];
                }
            }

            return min;
        }

        public TimeGranularity? GetMaxStep(TimeGranularity?[] steps)
        {
            var hasValue = steps.Where(s => s.HasValue);

            if (hasValue.Count() == 0)
            {
                return null;
            }

            return this.GetMaxStep(hasValue.Select(t => t.Value).ToArray());
        }
    }
}