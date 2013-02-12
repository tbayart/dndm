using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDownloader.Core.Common
{
    public static class DateTimeExtension
    {
        public static int ToUnixTime(this DateTime time)
        {
            TimeSpan span = (time - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }

        public static DateTime FromUnixTime(double timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddSeconds(timestamp);
            return newDateTime.ToUniversalTime();
        }
    }
}
