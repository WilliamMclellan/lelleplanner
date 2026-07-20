using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public static class AchievementTracker
    {
        private static readonly Dictionary<string, string> AchievementSources = new()
        {
            ["habit-handled"] = "habits-maintained",
            ["plates-for-days"] = "mythical-kitchen",
            ["tidy-home-tidy-life"] = "always-ready",
        };

        public static void HandleQuestCompleted(QuestCompleted evt, GameState gameState)
        {
            if (AchievementSources.TryGetValue(evt.QuestKey, out var achievementKey))
            {
                var achievement = gameState.Achievements.FirstOrDefault(a => a.Key == achievementKey);
                if (achievement != null && achievement.LifetimeCount < achievement.Threshold)
                {
                    achievement.LifetimeCount++;

                    if (achievement.LifetimeCount >= achievement.Threshold && !achievement.Completed)
                    {
                        achievement.Completed = true;
                        gameState.MarkovFragments++;
                    }
                }
            }
        }
    }
}
