using System;
using System.Globalization;

namespace LuviKunG.Date
{
    public static class DateTimeExtension
    {
        private const string DATETIME_FORMAT_ISO8601 = "yyyy-MM-ddTHH\\:mm\\:ssK";

        public static string ISO8601(this DateTime dateTime) => dateTime.ToString(DATETIME_FORMAT_ISO8601, CultureInfo.InvariantCulture);
    }
}