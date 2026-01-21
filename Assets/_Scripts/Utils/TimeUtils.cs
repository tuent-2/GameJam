using System;
using UnityEngine;

public static class TimeUtils
{
    public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToLocalTime();
    }

    public static DateTime UnixTimeStampToDateTimeMilliseconds(long unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimeStamp);
        return dateTime.ToLocalTime();
    }

    public static string GetDateTimeFromSeconds(long seconds)
    {
        var day = seconds / (3600 * 24);
        var hours = seconds / 3600;
        var minutes = seconds / 60;
        var sec = seconds % 60;

        var dayStr = day < 10 ? "0" + day : day.ToString();
        var hourStr = hours % 24 < 10 ? "0" + (hours % 24) : (hours % 24).ToString();
        var minutesStr = minutes % 60 < 10 ? "0" + (minutes % 60) : (minutes % 60).ToString();
        var secStr = sec < 10 ? "0" + sec : sec.ToString();

        if (day > 0)
        {
            return $"{dayStr} ngày {hourStr}:{minutesStr}:{secStr}";
        }

        if (hours > 0)
        {
            return $"{hourStr}:{minutesStr}:{secStr}";
        }

        if (minutes > 0)
        {
            return $"00:{minutesStr}:{secStr}";
        }

        if (sec > 0)
        {
            return $"00:00:{secStr}";
        }

        return "00:00:00";
    }
}