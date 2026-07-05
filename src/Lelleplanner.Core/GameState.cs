using System;
using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public class GameState
    {
        public DateOnly GameDate { get; set; }
        public DateOnly WeekStartDate { get; set; }

        public int DailyCoins { get; set; }
        public int WeeklyCoins { get; set; }
        public List<Quest> Quests { get; set; }
        public List<Quest> WeeklyQuests { get; set; }

        public GameState()
        {
            GameDate = GameClock.GetGameDate();
            WeekStartDate = GameClock.GetGameWeekStart();
            DailyCoins = 0;
            WeeklyCoins = 0;
            Quests = InitializeDefaultQuests();
            WeeklyQuests = InitializeWeeklyQuests();
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

        public void RolloverIfNeeded()
        {
            var currentGameDate = GameClock.GetGameDate();
            if ( GameDate != currentGameDate )
            {
                foreach (Quest quest in Quests)
                {
                    quest.Completed = false;
                }
                GameDate = currentGameDate;
            }
        }

        public void WeeklyRolloverIfNeeded()
        {
            var currentWeekStartDate = GameClock.GetGameWeekStart();
            if (WeekStartDate != currentWeekStartDate)
            {
                foreach (Quest quest in WeeklyQuests)
                {
                    quest.Completed = false;
                }
                WeekStartDate = currentWeekStartDate;
            }
        }
    }
}
