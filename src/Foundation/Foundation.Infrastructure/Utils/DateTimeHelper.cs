/*------------------------------Summary------------------------------------------------------
 * Product Name : REM
 * File Name	: DateTimeHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Datetime utility
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Linq;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// Date time utility class.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// The c++ base date time whose value is 1970/01/01 00:00:00
        /// </summary>
        /// <remarks>The <see cref="DateTime.Kind" /> is <see cref="DateTime.Kind.Utc" /></remarks>
        public static DateTime BaseDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Convert c++ time_t to .NET datetime.
        /// </summary>
        /// <param name="second">second of c++ time_t.</param>
        /// <returns>.NET datatime.</returns>
        /// <remarks>The <see cref="DateTime.Kind" /> of the return value will be <see cref="DateTime.Kind.Unspecified" /></remarks>
        public static DateTime Time_t2DateTime(long second)
        {
            return BaseDateTime.AddSeconds(second);
        }

        /// <summary>
        /// Convert .NET datetime to c++ time_t.
        /// </summary>
        /// <param name="dateTime">.NET datetime.</param>
        /// <returns>second of c++ time_t.</returns>
        public static long DateTime2Time_t(DateTime dateTime)
        {
            return Convert.ToInt64(((TimeSpan)(dateTime - BaseDateTime)).TotalSeconds);
        }

        /// <summary>
        /// Convert local time to UTC time.
        /// </summary>
        /// <param name="localDateTime">Local time.</param>
        /// <param name="offset">The offset between local time zone and UTC time zone.</param>
        /// <returns>A <see cref="DateTime" /> object whose value is the UTC equivalent to the value of the localDateTime object.</returns>
        /// <remarks>The <see cref="DateTime.Kind" /> of the return value will be same with the localDateTime parameter.</remarks>
        public static DateTime LocalTime2UtcTime(DateTime localDateTime, decimal offset)
        {
            return localDateTime.AddHours(Convert.ToDouble(-offset));
        }

        /// <summary>
        /// Convert UTC time to local time.
        /// </summary>
        /// <param name="utcDateTime">UTC time.</param>
        /// <param name="offset">The offset between local time zone and UTC time zone.</param>
        /// <returns>A <see cref="DateTime" /> object whose value is the local equivalent to the value of the utcDateTime object.</returns>
        /// <remarks>The <see cref="DateTime.Kind" /> of the return value will be same with the utcDateTime parameter.</remarks>
        public static DateTime UtcTime2LocalTime(DateTime utcDateTime, decimal offset)
        {
            return utcDateTime.AddHours(Convert.ToDouble(offset));
        }

        /// <summary>
        /// Get the first date time of any nullable time at defined step.
        /// </summary>
        /// <param name="anyTime">any nullable time.</param>
        /// <returns>the first minute of this time or null.</returns>
        public static DateTime? GetFirstDateTime(DateTime? anyTime, TimeGranularity step)
        {
            if (anyTime.HasValue)
            {
                return GetFirstDateTime(anyTime.Value, step);
            }
            else
                return null;
        }

        /// <summary>
        /// Get the first date time of anytime at defined step.
        /// </summary>
        /// <param name="anyTime">any time.</param>
        /// <returns>the first minute of this time.</returns>
        public static DateTime GetFirstDateTime(DateTime anyTime, TimeGranularity step)
        {
            switch (step)
            {
                case TimeGranularity.Minite:
                    return GetMinuteFirst(anyTime, 1);
                case TimeGranularity.Hourly:
                    return GetHourFirstMinute(anyTime);
                case TimeGranularity.Daily:
                    return GetDayFirstHour(anyTime);
                case TimeGranularity.Monthly:
                    return GetMonthFirstDay(anyTime);
                case TimeGranularity.Yearly:
                    return GetYearFirstDay(anyTime);
                case TimeGranularity.Weekly:
                    return GetWeekFirstDay(anyTime);
                case TimeGranularity.Min15:
                    return GetMinuteFirst(anyTime, 15);
                case TimeGranularity.Min30:
                    return GetMinuteFirst(anyTime, 30);

                default:
                    throw new ArgumentOutOfRangeException("Not supported step in generating first time.");
            }
        }

        /// <summary>
        /// Get the first minute of anytime.
        /// </summary>
        /// <param name="anyTime">any time.</param>
        /// <returns>the first minute of this time.</returns>
        private static DateTime GetHourFirstMinute(DateTime anyTime)
        {
            return anyTime.Date.AddHours(anyTime.Hour);
        }

        /// <summary>
        /// Get the first minute of anytime.
        /// </summary>
        /// <param name="anyTime">any time.</param>
        /// <returns>the first minute of this time.</returns>
        //private static DateTime GetRawFirstMinute(DateTime anyTime)
        //{
        //    return anyTime.AddMinutes(0 - (anyTime.Minute % 15)).AddSeconds(0 - anyTime.Second);
        //}

        private static DateTime GetMinuteFirst(DateTime anyTime,int minsCount)
        {
            return anyTime.AddMinutes(0 - (anyTime.Minute % minsCount)).AddSeconds(0 - anyTime.Second);
        }

        /// <summary>
        /// Get the first hour of one day.
        /// </summary>
        /// <param name="anyTime">any time.</param>
        /// <returns>The first hour of this day.</returns>
        private static DateTime GetDayFirstHour(DateTime anyTime)
        {
            return anyTime.Date;
        }

        /// <summary>
        /// Get first week day of one time.
        /// </summary>
        /// <param name="anyTime">Any time.</param>
        /// <returns>Monday of this time.</returns>
        public static DateTime GetWeekFirstDay(DateTime anyTime)
        {
            var adddays = 1 - Convert.ToInt32(anyTime.DayOfWeek);
            if (anyTime.DayOfWeek == DayOfWeek.Sunday) adddays = -6;
            return anyTime.Date.AddDays(adddays);
        }

        /// <summary>
        /// Get first day of one month.
        /// </summary>
        /// <param name="anyTime">Any time.</param>
        /// <returns>The first day of this month.</returns>
        private static DateTime GetMonthFirstDay(DateTime anyTime)
        {
            return anyTime.Date.AddDays(-anyTime.Day + 1);
        }

        /// <summary>
        /// Get first day of one quarter.
        /// </summary>
        /// <param name="anyTime">Any time.</param>
        /// <returns>The first day of this quarter.</returns>
        private static DateTime GetQuarterFirstDay(DateTime anyTime)
        {
            return anyTime.Date.AddMonths(0 - (anyTime.Month - 1) % 3).AddDays(1 - anyTime.Day);
        }

        /// <summary>
        /// Get first day of one year.
        /// </summary>
        /// <param name="anyTime">Any time.</param>
        /// <returns>The first day of this year.</returns>
        private static DateTime GetYearFirstDay(DateTime anyTime)
        {
            return anyTime.Date.AddDays(-anyTime.DayOfYear + 1);
        }

        public static TimeRange GetTimeRangeByRelativeDate(DateTime baseTime, RelativeDateType relativeDateType)
        {
            var startTime = baseTime;
            var endTime = baseTime;

            switch (relativeDateType)
            {
                case RelativeDateType.Last7Day:
                    startTime = baseTime.Date.AddDays(-6);
                    endTime = baseTime.Date.AddDays(1);

                    break;
                case RelativeDateType.Today:
                    startTime = baseTime.Date;
                    endTime = baseTime.Date.AddDays(1);

                    break;
                case RelativeDateType.Yesterday://
                    startTime = baseTime.Date.AddDays(-1);
                    endTime = baseTime.Date;

                    break;
                case RelativeDateType.ThisWeek:
                    startTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Weekly);

                    endTime = startTime.Date.AddDays(7);

                    break;
                case RelativeDateType.LastWeek://
                    endTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Weekly);
                    startTime = endTime.AddDays(-7);

                    break;
                case RelativeDateType.ThisMonth:
                    startTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Monthly);

                    endTime = startTime.AddMonths(1);

                    break;
                case RelativeDateType.LastMonth://
                    endTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Monthly);
                    startTime = endTime.AddMonths(-1);

                    break;
                case RelativeDateType.ThisYear:
                    startTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Yearly);

                    endTime = startTime.AddYears(1);

                    break;
                case RelativeDateType.LastYear://
                    endTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Yearly);
                    startTime = endTime.AddYears(-1);

                    break;
                case RelativeDateType.Last30Day:
                    startTime = baseTime.Date.AddDays(-29);

                    endTime = startTime.Date.AddDays(30);

                    break;
                case RelativeDateType.Last12Month:
                    //startTime = baseTime.Date.AddMonths(-11);
                    startTime = DateTimeHelper.GetFirstDateTime(baseTime.Date, TimeGranularity.Monthly).AddMonths(-11);

                    endTime = startTime.AddMonths(12);

                    break;
            }

            return new TimeRange() { StartTime = startTime, EndTime = endTime };
        }

        public static TimeRange[] GetTimeRangesByRelativeDate(TimeRange[] timeRanges, DateTime baseTime, RelativeDateType relativeDateType)
        {
            TimeRange relativeTR = GetTimeRangeByRelativeDate(baseTime, relativeDateType);

            var delta = relativeTR.StartTime - timeRanges[0].StartTime;

            var relativeTRs = timeRanges.Select(tr =>
            {
                return new TimeRange()
                {
                    StartTime = tr.StartTime.Add(delta),
                    EndTime = tr.StartTime.Add(delta).Add(relativeTR.EndTime - relativeTR.StartTime)
                };
            });

            return new TimeRange[] { relativeTR }.Union(relativeTRs.Skip(1)).ToArray();
        }

        /// <summary>
        /// Add steps to time by TimeGranularity
        /// </summary>
        /// <param name="time">The original time, if the TimeGranularity is weekly, this parameter should be the first day of week</param>
        /// <param name="step">the TimeGranularity</param>
        /// <param name="count">The count of steps to be added</param>
        /// <returns></returns>
        public static DateTime AddStep(DateTime time,TimeGranularity step,int count = 1)
        {
            switch (step)
            {
                case TimeGranularity.Minite:
                    time = time.AddMinutes(15 * count);
                    break;
                case TimeGranularity.Min15:
                    time = time.AddMinutes(15 * count);
                    break;
                case TimeGranularity.Min30:
                    time = time.AddMinutes(30 * count);
                    break;
                case TimeGranularity.Hourly:
                    time = time.AddHours(1 * count);
                    break;
                case TimeGranularity.Daily:
                    time = time.AddDays(1 * count);
                    break;
                case TimeGranularity.Weekly: //asume the input time is already the first day of week
                    time = time.AddDays(7 * count);
                    break;
                case TimeGranularity.Monthly:
                    time = time.AddMonths(1 * count);
                    break;
                case TimeGranularity.Yearly:
                    time = time.AddYears(1 * count);
                    break;
            }

            return time;
        }

        /// <summary>
        /// Add steps to time by TimeGranularity
        /// </summary>
        /// <param name="time">The original time, if the TimeGranularity is weekly, this parameter should be the first day of week</param>
        /// <param name="step">the TimeGranularity</param>
        /// <param name="count">The count of steps to be added</param>
        /// <returns></returns>
        public static DateTime GetNextStepDateTime(DateTime time, TimeGranularity step, int count = 1)
        {
            DateTime nextTime = time;
            switch (step)
            {
                case TimeGranularity.Minite:
                    nextTime = time.AddMinutes(15 * count);
                    break;
                case TimeGranularity.Min15:
                    nextTime = time.AddMinutes(15 * count);
                    break;
                case TimeGranularity.Min30:
                    nextTime = time.AddMinutes(30 * count);
                    break;
                case TimeGranularity.Hourly:
                    nextTime = time.AddHours(1 * count);
                    break;
                case TimeGranularity.Daily:
                    nextTime = time.AddDays(1 * count);
                    break;
                case TimeGranularity.Weekly: //asume the input time is already the first day of week
                    nextTime = time.AddDays(7 * count);
                    break;
                case TimeGranularity.Monthly:
                    nextTime = time.AddMonths(1 * count);
                    break;
                case TimeGranularity.Yearly:
                    nextTime = time.AddYears(1 * count);
                    break;
            }

            return nextTime;
        }
    }
}