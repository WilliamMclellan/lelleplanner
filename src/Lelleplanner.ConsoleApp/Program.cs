using Lelleplanner.ConsoleApp;
using Lelleplanner.Core;
using System;
using System.Linq;

var gameState = Persistence.LoadOrCreate();
var gameEngine = new GameEngine();
var totalQuestList = gameState.Quests.Where(quest => quest.Key != "daily-quest-clear").ToList();

while ( true )
{
    var activeQuestList = gameState.Quests
        .Where(q => !q.Completed && q.Key != "daily-quest-clear")
        .ToList();

    var completedQuestList = gameState.Quests
        .Where(q => q.Completed && q.Key != "daily-quest-clear")
        .ToList();

    ConsoleRenderer.RenderBanner(completedQuestList.Count(), totalQuestList.Count(), gameState.DailyCoins);
    ConsoleRenderer.RenderQuestList(activeQuestList, completedQuestList);

    if (!gameEngine.HasRemainingQuests(activeQuestList))
    {
        Console.WriteLine("You're all done! Amazing work!");
        Save(gameState);
        Console.WriteLine("Closing app...");
        break;
    }

    int questNumber = ConsoleRenderer.PromptForQuestNumber();
    while ( !gameEngine.ValidQuestNumber(activeQuestList.Count(), questNumber) && questNumber != 0)
    {
        questNumber = ConsoleRenderer.PromptForQuestNumber();
    }

    if ( gameEngine.ValidQuestNumber(activeQuestList.Count(), questNumber) )
    {
        bool? dailyQuestCleared = gameState.Quests.FirstOrDefault(q => q.Key == "daily-quest-clear")?.Completed;
        gameEngine.CompleteQuest(gameState, activeQuestList[questNumber - 1].Key);
        ConsoleRenderer.RenderQuestCompleted();

        activeQuestList = gameState.Quests
            .Where(q => !q.Completed && q.Key != "daily-quest-clear")
            .ToList();

        if (dailyQuestCleared != null && dailyQuestCleared != true && !gameEngine.HasRemainingQuests(activeQuestList))
        {
            gameEngine.CompleteDailyQuest(gameState);
            ConsoleRenderer.RenderDailyClearCelebration();
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