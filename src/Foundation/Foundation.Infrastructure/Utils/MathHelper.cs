using System;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public static class MathHelper
    {
        public static decimal? Round(double? value, int? precision)
        {
            if (precision.HasValue)
            {
                if (value.HasValue)
                {
                    return Convert.ToDecimal(Math.Round(value.Value, precision.Value, MidpointRounding.AwayFromZero));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return Convert.ToDecimal(value.Value);
            }
        }

        public static decimal? Round(decimal? value, int? precision)
        {
            if (precision.HasValue)
            {
                if (value.HasValue)
                {
                    return Math.Round(value.Value, precision.Value, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return value.Value;
            }
        }
    }
}