namespace Lelleplanner.Tests;

using Lelleplanner.Core;

public class GameClockTests
{
    [Theory]
    [MemberData(nameof(GameDateCases))]
    public void GetGameDate_Assert(DateTime input, DateOnly expected)
    {
        // Act
        var result = GameClock.GetGameDate(input);

        // Assert
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