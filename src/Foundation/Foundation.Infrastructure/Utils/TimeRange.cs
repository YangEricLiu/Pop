/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: TimeRange.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Time range
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public class TimeRange
    {
        private DateTime _startTime;
        private DateTime _endTime;

        public DateTime StartTime
        {
            get
            {
                this._startTime = DateTime.SpecifyKind(this._startTime, DateTimeKind.Utc);

                return this._startTime;
            }

            set
            {

                this._startTime = value;
            }
        }


        public DateTime EndTime
        {
            get
            {
                this._endTime = DateTime.SpecifyKind(this._endTime, DateTimeKind.Utc);

                return this._endTime;
            }

            set
            {
                this._endTime = value;
            }
        }

        public TimeRange(DateTime startTime, DateTime endTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public TimeRange()
        {
        }

        public TimeRange LocalTime2UtcTime()
        {
            return new TimeRange()
            {
                StartTime = this._startTime.AddHours(Convert.ToDouble(0 - ConstantValue.TimeOffSet)),
                EndTime = this._endTime.AddHours(Convert.ToDouble(0 - ConstantValue.TimeOffSet))
            };
        }

        public TimeRange UtcTime2LocalTime()
        {
            return new TimeRange()
            {
                StartTime = this._startTime.AddHours(Convert.ToDouble(ConstantValue.TimeOffSet)),
                EndTime = this._endTime.AddHours(Convert.ToDouble(ConstantValue.TimeOffSet))
            };
        }

        public Range Local2ToUtcRange()
        {
            return this.LocalTime2UtcTime().ToTickRange();
        }

        public Range ToTickRange()
        {
            return new Range()
            {
                Start = DateTimeHelper.DateTime2Time_t(this.StartTime),
                End = DateTimeHelper.DateTime2Time_t(this.EndTime),
            };
        }
    }

    public class Range
    {
        public long Start { get; set; }
        public long End { get; set; }
    }
}
