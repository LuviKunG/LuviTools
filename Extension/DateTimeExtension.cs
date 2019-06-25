using System;
using System.Globalization;

public static class DateTimeExtension
{
    private const string DATETIME_FORMAT_ISO8601 = "yyyy-MM-ddTHH\\:mm\\:ssK";

    public static string ISO8601(this DateTime dateTime) { return dateTime.ToString(DATETIME_FORMAT_ISO8601, CultureInfo.InvariantCulture); }
    // ==== Try this ====
    //DateTime time = DateTime.Now;
    //DateTime timeUTC = DateTime.UtcNow;
    //Debug.Log(time.ISO8601());
    //Debug.Log(timeUTC.ISO8601());
}
