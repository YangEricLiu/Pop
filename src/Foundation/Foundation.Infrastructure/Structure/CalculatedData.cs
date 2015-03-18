using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Structure
{
    public class CalculatedData
    {
        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                _startTime = value;

                _startTime = DateTime.SpecifyKind(_startTime, DateTimeKind.Utc);
            }
        }
        public decimal? Value { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder()
                .Append("Time: ")
                .Append(StartTime.ToString())
                .Append("; Value: ")
                .Append(Value.ToString());
            return sb.ToString();
        }
    }

}
