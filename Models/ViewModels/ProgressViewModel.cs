using System;

namespace Drink4Burpee.Models.ViewModels
{
    public class ProgressViewModel
    {
        public int Level { get; set; }

        public int CurrentBurpeeCount { get; set; }

        public int NextLevelBurpeeCount { get; set; }
    }
}