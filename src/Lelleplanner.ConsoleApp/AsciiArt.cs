namespace Lelleplanner.ConsoleApp
{
    public static class AsciiArt
    {
        public static string Separator = "===|==|==|==|==|==|==|==|==|======|==|==|==|==|==|==|==|===";
        public const string Banner = """
============================================================
||                                                        ||
||          L E L L E P L A N N E R                       ||
||                                                        ||
||     Daily Quests  *  Daily Coins  *  Daily Progress    ||
||                                                        ||
============================================================
""";
        public const string Celebration = """
                  ___________
                 '._==_==_=_.'
                 .-\:      /-.
                | (|:.     |) |
                 '-|:.     |-'
                   \::.    /
                    '::. .'
                      ) (
                    _.' '._
                   `-------`

        *  ALL DAILY QUESTS CLEARED!  *
              +1 Daily Coin!
""";
        public const string WeeklyCelebration = """
                   ___________
                  '.  \   /  .'
                    \  \ /  /
                     \  V  /
                      |   |
                      |___|
                     /     \
                    /_______\
                    `-------'

        *  ALL WEEKLY QUESTS CLEARED!  *
              +1 Weekly Coin!
""";

        public static string QuestCompleted = """
            ============================================================
            ||                                                        ||
            ||                    QUEST COMPLETED!                    ||
            ||                                                        ||
            ============================================================
            """;
    }
}
