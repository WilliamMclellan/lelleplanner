using Lelleplanner.ConsoleApp;
using Lelleplanner.Core;
using System;
using System.Collections.Generic;
using System.Linq;

var gameState = Persistence.LoadOrCreate();
GameEngine.OnQuestCompleted += evt => MonthlyProgressTracker.HandleQuestCompleted(evt, gameState);
var totalDailyQuestList = gameState.DailyQuests.Where(quest => quest.Key != "daily-quest-clear").ToList();
var totalWeeklyQuestList = gameState.WeeklyQuests.Where(quest => quest.Key != "weekly-quest-clear").ToList();

while ( true )
{
    var activeDailyQuestList = gameState.DailyQuests
        .Where(q => !q.Completed && q.Key != "daily-quest-clear")
        .ToList();

    var completedDailyQuestList = gameState.DailyQuests
        .Where(q => q.Completed && q.Key != "daily-quest-clear")
        .ToList();

    var activeWeeklyQuestList = gameState.WeeklyQuests
        .Where(q => !q.Completed && q.Key != "weekly-quest-clear")
        .ToList();

    var completedWeeklyQuestList = gameState.WeeklyQuests
        .Where(q => q.Completed && q.Key != "weekly-quest-clear")
        .ToList();

    var activeQuestList = activeDailyQuestList
        .Concat(activeWeeklyQuestList)
        .ToList();

    ConsoleRenderer.RenderBanner(completedDailyQuestList.Count(), totalDailyQuestList.Count(), gameState.DailyCoins,
        completedWeeklyQuestList.Count(), totalWeeklyQuestList.Count(), gameState.WeeklyCoins);

    List<(Quest Quest, string Label)> activeQuestListWithLabels = activeDailyQuestList
        .Select(quest => (quest, "Daily"))
        .Concat(activeWeeklyQuestList.Select(quest => (quest, "Weekly")))
        .ToList();

    List<(Quest Quest, string Label)> completedQuestListWithLabels = completedDailyQuestList
        .Select(quest => (quest, "Daily"))
        .Concat(completedWeeklyQuestList.Select(quest => (quest, "Weekly")))
        .ToList();

    ConsoleRenderer.RenderQuestList(activeQuestListWithLabels, completedQuestListWithLabels);

    if (!GameEngine.HasRemainingQuests(activeQuestList))
    {
        Console.WriteLine("You're all done! Amazing work!");
        Save(gameState);
        Console.WriteLine("Closing app...");
        break;
    }

    int questNumber = ConsoleRenderer.PromptForQuestNumber();
    while (!GameEngine.ValidQuestNumber(activeQuestList.Count(), questNumber) && questNumber != 0)
    {
        questNumber = ConsoleRenderer.PromptForQuestNumber();
    }

    if (GameEngine.ValidQuestNumber(activeQuestList.Count(), questNumber))
    {
        bool? dailyQuestCleared = gameState.DailyQuests.FirstOrDefault(q => q.Key == "daily-quest-clear")?.Completed;
        bool? weeklyQuestCleared = gameState.WeeklyQuests.FirstOrDefault(q => q.Key == "weekly-quest-clear")?.Completed;

        GameEngine.CompleteQuest(gameState, activeQuestList[questNumber - 1].Key);

        activeDailyQuestList = gameState.DailyQuests
                .Where(q => !q.Completed && q.Key != "daily-quest-clear")
                .ToList();

        activeWeeklyQuestList = gameState.WeeklyQuests
                .Where(q => !q.Completed && q.Key != "weekly-quest-clear")
                .ToList();

        ConsoleRenderer.RenderQuestCompleted();

        if (dailyQuestCleared != null && dailyQuestCleared != true && !GameEngine.HasRemainingQuests(activeDailyQuestList))
        {
            GameEngine.CompleteDailyQuest(gameState);
            ConsoleRenderer.RenderDailyClearCelebration();
        }

        if (weeklyQuestCleared != null && weeklyQuestCleared != true && !GameEngine.HasRemainingQuests(activeWeeklyQuestList))
        {
            GameEngine.CompleteWeeklyQuest(gameState);
            ConsoleRenderer.RenderWeeklyClearCelebration();
        }

        Save(gameState);
    }

    if (questNumber == 0)
    {
        Save(gameState);
        Console.WriteLine("Closing app...");
        break;
    }

}

static void Save(GameState gameState)
{
    Console.WriteLine("Saving...");
    Persistence.Save(gameState);
    Console.WriteLine("File Saved!");
}