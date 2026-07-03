using System;
using System.Collections.Generic;
using System.Linq;

namespace Lelleplanner.Core
{
    public class GameState
    {
        public DateOnly GameDate { get; set; }
        public int DailyCoins { get; set; }
        public List<Quest> Quests { get; set; }

        public GameState()
        {
            GameDate = GameClock.GetGameDate();
            DailyCoins = 0;
            Quests = InitializeDefaultQuests();
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

        public void RolloverIfNeeded()
        {
            if ( GameDate != GameClock.GetGameDate() )
            {
                foreach (Quest quest in Quests)
                {
                    quest.Completed = false;
                }
            }
        }
    }
}
