using Lelleplanner.Core;
namespace Lelleplanner.Tests
{
    public class MonthlyProgressTrackerTests
    {
        [Fact]
        public void HandleQuestCompleted_ProgressIncreased()
        {
            var gameState = new GameState();

            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);

            Assert.Equal(1, gameState.MonthlyQuests.First(q => q.Key == "plates-for-days").Progress);
        }

        [Fact]
        public void HandleQuestCompleted_ThresholdReachedQuestComplete()
        {
            var gameState = new GameState();

            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);

            Assert.Equal(4, gameState.MonthlyQuests.First(q => q.Key == "plates-for-days").Progress);
            Assert.True(gameState.MonthlyQuests.First(q => q.Key == "plates-for-days").Completed);
        }

        [Fact]
        public void HandleQuestCompleted_NoDoubleCompletes()
        {
            var gameState = new GameState();

            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);
            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("shiny-sparkly"), gameState);

            Assert.Equal(4, gameState.MonthlyQuests.First(q => q.Key == "plates-for-days").Progress);
            Assert.True(gameState.MonthlyQuests.First(q => q.Key == "plates-for-days").Completed);
        }

        [Fact]
        public void HandleQuestCompleted_NoProgressIncreased()
        {
            var gameState = new GameState();

            MonthlyProgressTracker.HandleQuestCompleted(new QuestCompleted("buff-papaya"), gameState);

            Assert.Equal(0, gameState.MonthlyQuests.Count(q => q.Progress > 0));
        }
    }
}
