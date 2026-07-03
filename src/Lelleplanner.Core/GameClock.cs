using System;

namespace Lelleplanner.Core
{
    public static class GameClock
    {
        private const int CutoverHour = 4;

        public static DateOnly GetGameDate(DateTime now)
        { 
            if ( now.Hour < CutoverHour)
            {
                return new DateOnly(now.Year, now.Month, now.Day - 1);
            }
            return new DateOnly(now.Year, now.Month, now.Day);
        }

        public static DateOnly GetGameDate()
        {
            return GetGameDate(DateTime.Now);
        }
    }
}
