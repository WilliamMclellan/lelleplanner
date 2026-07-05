namespace Lelleplanner.Tests;

using Lelleplanner.Core;

public class GameClockTests
{
    [Theory]
    [MemberData(nameof(GameDateCases))]
    public void GetGameDate_Assert(DateTime input, DateOnly expected)
    {
        var result = GameClock.GetGameDate(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetGameWeekStart_BeforeRollover_PreviousWeek()
    {
        var dateTime = new DateTime(2026, 7, 6, 2, 0, 0);
        var expected = new DateOnly(2026, 6, 29);

        var result = GameClock.GetGameWeekStart(dateTime);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetGameWeekStart_NewWeek()
    {
        var dateTime = new DateTime(2026, 7, 6, 4, 0, 0);
        var expected = new DateOnly(2026, 7, 6);

        var result = GameClock.GetGameWeekStart(dateTime);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetGameWeekStart_CurrentWeek()
    {
        var dateTime = new DateTime(2026, 7, 1, 4, 0, 0);
        var expected = new DateOnly(2026, 6, 29);

        var result = GameClock.GetGameWeekStart(dateTime);

        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> GameDateCases()
    {
        yield return new object[] { new DateTime(2026, 7, 4, 2, 0, 0), new DateOnly(2026, 7, 3) };
        yield return new object[] { new DateTime(2026, 7, 4, 15, 0, 0), new DateOnly(2026, 7, 4) };
        yield return new object[] { new DateTime(2026, 7, 4, 3, 0, 0), new DateOnly(2026, 7, 3) };
        yield return new object[] { new DateTime(2026, 7, 1, 2, 0, 0), new DateOnly(2026, 6, 30) };
        yield return new object[] { new DateTime(2026, 1, 1, 2, 0, 0), new DateOnly(2025, 12, 31) };
    }
}