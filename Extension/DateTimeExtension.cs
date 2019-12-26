using System;
using System.Globalization;

namespace LuviKunG.Date
{
    public static class DateTimeExtension
    {
        private const string DATETIME_FORMAT_ISO8601 = "yyyy-MM-ddTHH\\:mm\\:ssK";
        private const string DATETIME_FORMAT_ISO8601_MILLISECOND = "yyyy-MM-ddTHH\\:mm\\:ss.fffK";

        public static string ToISO8601(this DateTime dateTime) => dateTime.ToString(DATETIME_FORMAT_ISO8601, CultureInfo.InvariantCulture);
        public static string ToISO8601Millisecond(this DateTime dateTime) => dateTime.ToString(DATETIME_FORMAT_ISO8601_MILLISECOND, CultureInfo.InvariantCulture);
    }
}