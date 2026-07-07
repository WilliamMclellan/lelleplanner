using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public static class MonthlyProgressTracker
    {
        private static readonly Dictionary<string, string> MonthlyQuestSources = new()
        {
            ["daily-quest-clear"] = "habit-handled",
            ["shiny-sparkly"] = "plates-for-days",
            ["tidy-room-tidy-mind"] = "tidy-home-tidy-life",
        };

        public static void HandleQuestCompleted(QuestCompleted evt, GameState gameState) 
        {
            if (MonthlyQuestSources.TryGetValue(evt.QuestKey, out var monthlyQuestKey))
            {
                var quest = gameState.MonthlyQuests.FirstOrDefault(q => q.Key == monthlyQuestKey);
                if (quest != null && quest.Progress < quest.Threshold)
                {
                    quest.Progress++;

                    if (quest.Progress >= quest.Threshold && !quest.Completed)
                    {
                        quest.Completed = true;
                    }
                }
            }
        }
    }
}
