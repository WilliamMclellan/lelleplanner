using System;
using System.Collections.Generic;
using System.Linq;
using Lelleplanner.Core;

namespace Lelleplanner.ConsoleApp
{
    public static class ConsoleRenderer
    {
        
        public static void RenderBanner(int completedDailyQuests, int totalDailyQuests, int dailyCoins, 
            int completedWeeklyQuests, int totalWeeklyQuests, int weeklyCoins, int markovFragments)
        {
            Console.WriteLine(AsciiArt.Banner);
            var dailyGameInfo = $"Daily Coins: {dailyCoins} || Daily Quests Completed: {completedDailyQuests} / {totalDailyQuests}";
            Console.WriteLine(dailyGameInfo);
            var weeklyGameInfo = $"Weekly Coins: {weeklyCoins} || Weekly Quests Completed: {completedWeeklyQuests} / {totalWeeklyQuests}";
            Console.WriteLine(weeklyGameInfo);
            var markovFragmentInfo = $"Markov Fragments: {markovFragments}/3";
            Console.WriteLine(markovFragmentInfo);
            Console.WriteLine(AsciiArt.Separator);
        }

        public static void RenderQuestList(List<(Quest Quest, string Label)> activeQuestList, List<(Quest Quest, string Label)> completedQuestList)
        {
            Console.WriteLine("Daily & Weekly Quests:");
            Console.WriteLine("== Active ==");
            int i = 0;
            foreach( var quest in activeQuestList )
            {
                i++;
                Console.WriteLine($"{i}) || [{quest.Label}] {quest.Quest.Title}");
            }
            Console.WriteLine("== Completed ==");
            foreach (var quest in completedQuestList)
            {
                Console.WriteLine($"[CLEAR] || [{ quest.Label}] {quest.Quest.Title}");
            }
        }

        public static void RenderMonthlyList(List<MonthlyQuest> monthlyQuestList)
        {
            Console.WriteLine("Monthly Quests:");
            foreach(var quest in monthlyQuestList)
            {
                if (quest.Completed)
                {
                    Console.WriteLine($"[COMPLETED] || {quest.Title}");
                }
                else
                {
                    Console.WriteLine($"[{quest.Progress}/{quest.Threshold}] || {quest.Title}");
                }
            }
        }

        public static void RenderAchievements(List<Achievement> achievementList)
        {
            Console.WriteLine("Achievements:");
            foreach (var achievement in achievementList)
            {
                if (achievement.Completed)
                {
                    Console.WriteLine($"[COMPLETED] || {achievement.Title}");
                }
                else
                {
                    Console.WriteLine($"[{achievement.LifetimeCount}/{achievement.Threshold}] || {achievement.Title}");
                }
            }
        }

        public static void RenderDailyClearCelebration()
        {
            Console.WriteLine(AsciiArt.Celebration);
        }

        public static void RenderWeeklyClearCelebration()
        {
            Console.WriteLine(AsciiArt.WeeklyCelebration);
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
