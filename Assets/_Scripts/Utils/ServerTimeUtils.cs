using System;
using UnityEngine;

namespace Game.HotUpdateScripts.Utils
{
    public static class ServerTimeUtils
    {
        public static DateTime CurrentTime =>
            TimeUtils.UnixTimeStampToDateTimeMilliseconds(
                (long)(Time.realtimeSinceStartup - _REAL_TIME_SINCE_START_UP) * 1000 + _SERVER_TIME);

        private static long _SERVER_TIME;
        private static float _REAL_TIME_SINCE_START_UP;

        public static void UpdateTime(long serverTime)
        {
            _SERVER_TIME = serverTime;
            _REAL_TIME_SINCE_START_UP = Time.realtimeSinceStartup;
        }

        public static float GetEpochTimeToCurrentInSeconds(long epochMilliseconds)
        {
            return (float)(CurrentTime - TimeUtils.UnixTimeStampToDateTimeMilliseconds(epochMilliseconds))
                .TotalMilliseconds / 1000;
        }
    }
}