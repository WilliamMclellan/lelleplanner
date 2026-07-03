using System;

namespace Lelleplanner.Core
{
    public class Quest
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public bool Completed { get; set; }

        public Quest(string key, string title, string goal, bool completed = false)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Goal = goal ?? throw new ArgumentNullException(nameof(goal));
            Completed = completed;
        }
    }
}