using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Util
{
    public static class ServerUtil
    {
       public static long GetUnixFormatDateTime(DateTime date, TimestampFormatter formatter = TimestampFormatter.Milliseconds)
       {
            //var timeSpan = date.Subtract(new DateTime(1970, 1, 1,0,0,0, DateTimeKind.Utc));
            //return (Int32) timeSpan.TotalMilliseconds; //timeSpan.TotalMilliseconds;
            if (formatter == TimestampFormatter.Milliseconds)
                return ((DateTimeOffset)date).ToUnixTimeMilliseconds();
            else if (formatter == TimestampFormatter.Seconds)
                return ((DateTimeOffset)date).ToUnixTimeSeconds();
            else
                return 0;
        }
        public static DateTime GetDateTimeFromUnixFormat(double unixDate, TimestampFormatter formatter = TimestampFormatter.Milliseconds)
        {
            DateTime myDate = DateTime.MinValue;
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (formatter == TimestampFormatter.Milliseconds) 
                myDate = dt.AddMilliseconds(unixDate).ToLocalTime();
            else if (formatter == TimestampFormatter.Seconds)
                myDate = dt.AddSeconds(unixDate).ToLocalTime();
            return myDate;
        }
    }
}
