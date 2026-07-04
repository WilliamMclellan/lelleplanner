using System;
using System.Collections.Generic;
using System.Linq;
using Lelleplanner.Core;

namespace Lelleplanner.ConsoleApp
{
    public static class ConsoleRenderer
    {
        
        public static void RenderBanner(int completedQuests, int totalQuests, int dailyCoins)
        {
            Console.WriteLine(AsciiArt.Banner);
            var gameInfo = $"Daily Coins: {dailyCoins} || Daily Quests Completed: {completedQuests} / {totalQuests}";
            Console.WriteLine(gameInfo);
            Console.WriteLine(AsciiArt.Separator);
        }

        public static void RenderQuestList(List<Quest> activeQuestList, List<Quest> completedQuestList)
        {
            Console.WriteLine("Here are your current quests:");
            Console.WriteLine("== Active ==");
            var i = 0;
            foreach( Quest quest in activeQuestList )
            {
                i++;
                Console.WriteLine(i + ") || " + quest.Title);
            }
            Console.WriteLine("== Completed ==");
            foreach (Quest quest in completedQuestList)
            {
                Console.WriteLine("[CLEAR] || " + quest.Title);
            }
        }

        public static void RenderDailyClearCelebration()
        {
            Console.WriteLine(AsciiArt.Celebration);
        }

        public static int PromptForQuestNumber()
        {
            Console.WriteLine("Please type the number of the quest you'd like to complete.");
            Console.WriteLine("You may type 0 to close the app.");
            string? input = Console.ReadLine();
            int questNumber;
            while ( !int.TryParse(input, out questNumber) )
            {
                Console.WriteLine("Please provide a valid integer");
                input = Console.ReadLine();
            }
            return questNumber;
        }

        public static void RenderQuestCompleted()
        {
            Console.WriteLine(AsciiArt.QuestCompleted);
        }
    }
}
