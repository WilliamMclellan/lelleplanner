using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lelleplanner.Core
{
    public class GameEngine
    {
        public void CompleteQuest( GameState gameState, string questKey )
        {
            Quest? quest = gameState.Quests.FirstOrDefault(q => q.Key == questKey );
            if ( quest != null)
            {
                quest.Completed = true;
            }
        }

        public void CompleteDailyQuest( GameState gameState )
        {

            if ( gameState.Quests.Where(q => q.Key != "daily-quest-clear" ).All(q => q.Completed ) )
            {
                Quest? quest = gameState.Quests.FirstOrDefault(q => q.Key == "daily-quest-clear" && !q.Completed );
                if ( quest != null )
                {
                    quest.Completed = true;
                    gameState.DailyCoins++;
                }
            }
        }
    }
}
