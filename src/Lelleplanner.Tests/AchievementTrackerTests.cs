using Lelleplanner.Core;

namespace Lelleplanner.Tests
{
    public class AchievementTrackerTests
    {
        [Fact]
        public void HandleQuestCompleted_ProgressIncreased()
        {
            var gameState = new GameState();

            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);

            Assert.Equal(1, gameState.Achievements.First(q => q.Key == "mythical-kitchen").LifetimeCount);
        }

        [Fact]
        public void HandleQuestCompleted_ThresholdReachedQuestComplete()
        {
            var gameState = new GameState();

            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);

            Assert.Equal(4, gameState.Achievements.First(q => q.Key == "mythical-kitchen").LifetimeCount);
            Assert.True(gameState.Achievements.First(q => q.Key == "mythical-kitchen").Completed);
        }

        [Fact]
        public void HandleQuestCompleted_NoDoubleCompletes()
        {
            var gameState = new GameState();

            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);

            Assert.Equal(4, gameState.Achievements.First(q => q.Key == "mythical-kitchen").LifetimeCount);
            Assert.True(gameState.Achievements.First(q => q.Key == "mythical-kitchen").Completed);
        }

        [Fact]
        public void HandleQuestCompleted_NoProgressIncreased()
        {
            var gameState = new GameState();

            AchievementTracker.HandleQuestCompleted(new QuestCompleted("buff-papaya"), gameState);

            Assert.Equal(0, gameState.Achievements.Count(q => q.LifetimeCount > 0));
        }

        [Fact]
        public void HandleQuestCompleted_OneMarkovFragmentAwarded()
        {
            var gameState = new GameState();

            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);
            AchievementTracker.HandleQuestCompleted(new QuestCompleted("plates-for-days"), gameState);

            Assert.Equal(1, gameState.MarkovFragments);
        }
    }
}
