using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Structure
{
    /// <summary>
    /// Struct used to set UpdateUser and UpdateTime.
    /// </summary>
    public struct LastUpdateInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time"></param>
        public LastUpdateInfo(string user, DateTime time): this()
        {
            User = user;
            Time = time;
        }

        /// <summary>
        /// UpdateUser
        /// </summary>
        public string User { get; private set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Validate if <param name="info">info</param>> is null.
        /// </summary>
        public static bool IsNull(LastUpdateInfo info)
        {
            return String.IsNullOrEmpty(info.User) && info.Time == DateTime.MinValue;
        }
    }
}
