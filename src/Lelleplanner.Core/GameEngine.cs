using System;
using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public static class GameEngine
    {

        public static event Action<QuestCompleted>? OnQuestCompleted;

        public static void CompleteQuest(GameState gameState, string questKey)
        {
            Quest? quest = gameState.DailyQuests.FirstOrDefault(q => q.Key == questKey && !q.Completed)
                ?? gameState.WeeklyQuests.FirstOrDefault(q => q.Key == questKey && !q.Completed);

            if ( quest != null)
            {
                quest.Completed = true;
                OnQuestCompleted?.Invoke(new QuestCompleted(quest.Key));
            }
        }

        private static void CompleteMetaQuest(List<Quest> quests, string metaQuestKey, Action onCleared)
        {
            if (quests.Where(q => q.Key != metaQuestKey).All(q => q.Completed))
            {
                Quest? quest = quests.FirstOrDefault(q => q.Key == metaQuestKey && !q.Completed);
                if (quest != null)
                {
                    quest.Completed = true;
                    OnQuestCompleted?.Invoke(new QuestCompleted(quest.Key));
                    onCleared();
                }
            }
        }

        public static void CompleteDailyQuest(GameState gameState)
        {
            CompleteMetaQuest(gameState.DailyQuests, "daily-quest-clear", () => gameState.DailyCoins++);
        }

        public static void CompleteWeeklyQuest(GameState gameState)
        {
            CompleteMetaQuest(gameState.WeeklyQuests, "weekly-quest-clear", () => gameState.WeeklyCoins++);
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
