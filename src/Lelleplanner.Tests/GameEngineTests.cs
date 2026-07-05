using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Lelleplanner.Core;

namespace Lelleplanner.Tests
{
    public class GameEngineTests
    {
        [Fact]
        public void GameEngine_CompleteDailyQuest_GiveDailyCoin()
        {
            GameState gameState = new GameState();
            gameState.Quests.Where(q => !q.Completed && q.Key != "daily-quest-clear")
                .ToList()
                .ForEach(q => q.Completed = true);

            GameEngine.CompleteDailyQuest(gameState);

            Assert.Equal(1, gameState.DailyCoins);
            Assert.True(gameState.Quests.First(q => q.Key == "daily-quest-clear").Completed);
        }

        [Fact]
        public void GameEngine_CompleteDailyQuest_NoDoubleReward()
        {
            GameState gameState = new GameState();
            gameState.Quests.Where(q => !q.Completed && q.Key != "daily-quest-clear")
                .ToList()
                .ForEach(q => q.Completed = true);

            GameEngine.CompleteDailyQuest(gameState);
            GameEngine.CompleteDailyQuest(gameState);

            Assert.Equal(1, gameState.DailyCoins);
            Assert.True(gameState.Quests.First(q => q.Key == "daily-quest-clear").Completed);
        }

        [Fact]
        public void GameEngine_CompleteDailyQuest_NotDoneYet()
        {
            GameState gameState = new GameState();
            gameState.Quests.Where(q => !q.Completed && q.Key != "daily-quest-clear" && q.Key != "buff-papaya")
                .ToList()
                .ForEach(q => q.Completed = true);

            GameEngine.CompleteDailyQuest(gameState);

            Assert.Equal(0, gameState.DailyCoins);
            Assert.False(gameState.Quests.First(q => q.Key == "daily-quest-clear").Completed);
        }
    }
}
