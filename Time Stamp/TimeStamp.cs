using System;

public class TimeStamp
{
    public static long Now
    {
        get
        {
            long ticks = DateTime.Now.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            return ticks;
        }
    }

    public static long Today
    {
        get
        {
            long ticks = DateTime.Today.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            return ticks;
        }
    }

    public static long Tomorrow
    {
        get
        {
            long ticks = DateTime.Today.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            ticks += 86400;
            return ticks;
        }
    }

    public static long Yesterday
    {
        get
        {
            long ticks = DateTime.Today.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            ticks -= 86400;
            return ticks;
        }
    }

    public static long UTCNow
    {
        get
        {
            long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            return ticks;
        }
    }
}
