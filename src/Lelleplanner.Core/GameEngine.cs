using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public static class GameEngine
    {
        public static void CompleteQuest(GameState gameState, string questKey)
        {
            Quest? quest = gameState.Quests.FirstOrDefault(q => q.Key == questKey);
            if ( quest != null)
            {
                quest.Completed = true;
            }
        }

        public static void CompleteDailyQuest(GameState gameState)
        {
            if ( gameState.Quests.Where(q => q.Key != "daily-quest-clear" ).All(q => q.Completed ))
            {
                Quest? quest = gameState.Quests.FirstOrDefault(q => q.Key == "daily-quest-clear" && !q.Completed);
                if ( quest != null )
                {
                    quest.Completed = true;
                    gameState.DailyCoins++;
                }
            }
        }

        public static void CompleteWeeklyQuest(GameState gameState)
        {
            if (gameState.WeeklyQuests.Where(q => q.Key != "weekly-quest-clear").All(q => q.Completed))
            {
                Quest? quest = gameState.WeeklyQuests.FirstOrDefault(q => q.Key == "weekly-quest-clear" && !q.Completed);
                if (quest != null)
                {
                    quest.Completed = true;
                    gameState.WeeklyCoins++;
                }
            }
        }

        public static bool HasRemainingQuests(List<Quest> activeQuestList)
        {
            if (!activeQuestList.Any())
            {
                return false;
            }
            return true;
        }

        public static bool ValidQuestNumber(int activeQuests, int questNumber)
        {
            if (questNumber > activeQuests || questNumber < 1)
                return false;
            return true;
        }
    }
}
