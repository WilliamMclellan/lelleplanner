using System;

namespace Lelleplanner.Core
{
    public class MonthlyQuest
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public int Progress { get; set; }
        public int Threshold { get; set; }
        public bool Completed { get; set; }

        public MonthlyQuest(string key, string title, string goal, int progress = 0, int threshold = 0, bool completed = false)
        {
            Key = key ?? throw new ArgumentNullException(nameof(Key));
            Title = title ?? throw new ArgumentNullException(nameof(Title));
            Goal = goal ?? throw new ArgumentNullException(nameof(Goal));
            Progress = progress;
            Threshold = threshold;
            Completed = completed;
        }
    }
}
