using System;

namespace Lelleplanner.Core
{
    public class Achievement
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public int LifetimeCount { get; set; }
        public int Threshold { get; set; }
        public bool Completed { get; set; }

        public Achievement(string key, string title, string goal, int lifetimeCount = 0, int threshold = 0, bool completed = false)
        {
            Key = key ?? throw new ArgumentNullException(nameof(Key));
            Title = title ?? throw new ArgumentNullException(nameof(Title));
            Goal = goal ?? throw new ArgumentNullException(nameof(Goal));
            LifetimeCount = lifetimeCount;
            Threshold = threshold;
            Completed = completed;
        }
    }
}
