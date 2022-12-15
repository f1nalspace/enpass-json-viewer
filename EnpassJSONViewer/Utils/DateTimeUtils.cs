using System;

namespace EnpassJSONViewer.Utils
{
    static class DateTimeUtils
    {
        public static DateTime GetUnixTimeStamp(long value)
        {
            DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(value);
            return offset.UtcDateTime;
        }
    }
}
