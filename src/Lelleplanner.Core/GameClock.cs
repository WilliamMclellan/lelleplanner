using System;

namespace Lelleplanner.Core
{
    public static class GameClock
    {
        private const int CutoverHour = 4;

        public static DateOnly GetGameDate(DateTime now)
        {
            return now.Hour < CutoverHour
                ? DateOnly.FromDateTime(now.Date.AddDays(-1))
                : DateOnly.FromDateTime(now.Date);
        }

        public static DateOnly GetGameDate()
        {
            return GetGameDate(DateTime.Now);
        }

        public static DateOnly GetGameWeekStart(DateTime now)
        {
            var gameDate = GetGameDate(now);
            int daysSinceMonday = ((int)gameDate.DayOfWeek + 6) % 7;
            return gameDate.AddDays(-daysSinceMonday);
        }

        public static DateOnly GetGameWeekStart()
        {
            return GetGameWeekStart(DateTime.Now);
        }

        public static DateOnly GetGameMonthStart(DateTime now)
        {
            var gameDate = GetGameDate(now);
            return new DateOnly(gameDate.Year, gameDate.Month, 1);
        }

        public static DateOnly GetGameMonthStart()
        {
            return GetGameMonthStart(DateTime.Now);
        }
    }
}
