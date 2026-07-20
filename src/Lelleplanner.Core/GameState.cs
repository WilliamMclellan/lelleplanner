using System;
using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public class GameState
    {
        public DateOnly GameDate { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public DateOnly MonthStartDate { get; set; }

        public int DailyCoins { get; set; }
        public int WeeklyCoins { get; set; }
        public int MarkovFragments { get; set; }
        public List<Quest> DailyQuests { get; set; }
        public List<Quest> WeeklyQuests { get; set; }
        public List<MonthlyQuest> MonthlyQuests { get; set; }
        public List<Achievement> Achievements { get; set; }

        public GameState()
        {
            GameDate = GameClock.GetGameDate();
            WeekStartDate = GameClock.GetGameWeekStart();
            MonthStartDate = GameClock.GetGameMonthStart();
            DailyCoins = 0;
            WeeklyCoins = 0;
            MarkovFragments = 0;
            DailyQuests = InitializeDefaultQuests();
            WeeklyQuests = InitializeWeeklyQuests();
            MonthlyQuests = InitializeMonthlyQuests();
            Achievements = InitializeAchievements();
        }

        private List<Achievement> InitializeAchievements()
        {
            List<Achievement> achievements = new List<Achievement>
            {
                new Achievement("habits-maintained", "Habits Maintained!", "Clear 'Habit Handled!' 4 times", threshold: 4),
                new Achievement("mythical-kitchen", "Mythical Kitchen", "Clear 'Plates For Days' 4 times", threshold: 4),
                new Achievement("always-ready", "Always Ready!", "Clear 'Tidy Home, Tidy Life' 4 times", threshold: 4)
            };
            return achievements;
        }

        private List<Quest> InitializeDefaultQuests()
        {
            List<Quest> quests = new List<Quest>{ 
                new Quest("food-for-thought", "Food For Thought", "Eat breakfast & dinner"), 
                new Quest("shiny-pearly-whites", "Shiny Pearly Whites", "Brush teeth morning & night"), 
                new Quest("buff-papaya", "Buff Papaya", "Workout!"),
                new Quest("walk-for-life", "Walk For Life", "30min on the treadmill"),
                new Quest("pretty-boy-papaya", "Pretty Boy Papaya", "Wash face"),
                new Quest("smartypants-huh", "Smartypants Huh?", "Study for 30 minutes"),
                new Quest("daily-quest-clear", "Excellent work, daily cleared!", "Clear all 6 daily quests")
            };

            return quests;
        }

        private List<Quest> InitializeWeeklyQuests()
        {
            List<Quest> quests = new List<Quest>{
                new Quest("tidy-room-tidy-mind", "Tidy Room, Tidy Mind", "Clean a room"),
                new Quest("shiny-sparkly", "Shiny Sparkly!", "Do the dishes"),
                new Quest("weekly-quest-clear", "Look at you go, weekly cleared!", "Clear both weekly quests")
            };

            return quests;
        }

        private List<MonthlyQuest> InitializeMonthlyQuests()
        {
            List<MonthlyQuest> quests = new List<MonthlyQuest>
            {
                new MonthlyQuest("habit-handled", "Habit Handled!", "Clear 'Excellent work, daily cleared!' 25 times", threshold: 25),
                new MonthlyQuest("plates-for-days", "Plates For Days", "Clear 'Shiny Sparkly!' 4 times", threshold: 4),
                new MonthlyQuest("tidy-home-tidy-life", "Tidy Home, Tidy Life", "Clear 'Tidy Room, Tidy Mind' 4 times", threshold: 4)
            };

            return quests;
        }

        public void DailyRolloverIfNeeded()
        {
            GameDate = ResetQuestsIfNeeded(GameDate, GameClock.GetGameDate(), DailyQuests);
        }

        public void WeeklyRolloverIfNeeded()
        {
            WeekStartDate = ResetQuestsIfNeeded(WeekStartDate, GameClock.GetGameWeekStart(), WeeklyQuests);
        }

        public void MonthlyRolloverIfNeeded()
        {
            MonthStartDate = ResetProgressIfNeeded(MonthStartDate, GameClock.GetGameMonthStart(), MonthlyQuests);
        }

        private DateOnly ResetQuestsIfNeeded(DateOnly storedGameDate, DateOnly currentGameDate, List<Quest> quests)
        {
            if ( storedGameDate != currentGameDate )
            {
                foreach (Quest quest in quests)
                {
                    quest.Completed = false;
                }
                return currentGameDate;
            }
            return storedGameDate;
        }

        private DateOnly ResetProgressIfNeeded(DateOnly storedMonthStart, DateOnly currentMonthStart, List<MonthlyQuest> quests)
        {
            if ( storedMonthStart != currentMonthStart )
            {
                foreach (MonthlyQuest quest in quests)
                {
                    quest.Completed = false;
                    quest.Progress = 0;
                }
                return currentMonthStart;
            }

            return storedMonthStart;
        }
    }
}
