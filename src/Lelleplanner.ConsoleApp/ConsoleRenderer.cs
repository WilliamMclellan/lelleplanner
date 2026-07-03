using System;
using System.Linq;
using Lelleplanner.Core;

namespace Lelleplanner.ConsoleApp
{
    public static class ConsoleRenderer
    {
        public static string separator = "=== === === === === ===";
        public static void RenderBanner(GameState gameState)
        {
            Console.WriteLine(AsciiArt.Banner);
        }

        public static void RenderQuestList(GameState gameState)
        {
            Console.WriteLine("Here are your current quests:");
            Console.WriteLine("== Active ==");
            var i = 0;
            foreach( Quest quest in gameState.Quests.Where(quest => !quest.Completed).ToList() )
            {
                i++;
                Console.WriteLine(i + ") || " + quest.Title);
            }
            Console.WriteLine("== Completed ==");
            foreach (Quest quest in gameState.Quests.Where(quest => quest.Completed).ToList())
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
            string? input = Console.ReadLine();
            int questNumber;
            while ( !int.TryParse(input, out questNumber) )
            {
                Console.WriteLine("Please provide a valid integer");
                input = Console.ReadLine();
            }
            return questNumber;
        }
    }
}
