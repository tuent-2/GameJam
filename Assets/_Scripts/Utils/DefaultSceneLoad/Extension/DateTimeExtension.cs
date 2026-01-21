using System;

public static class DateTimeExtension
{
    public static long ToTimeStamp(this DateTime dateTime)
    {
        var span = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (long)span.TotalSeconds;
    }
    
    public static long TotalSeconds(this DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return (long)(diff.TotalSeconds);
    }
}