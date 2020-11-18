using System;

namespace Drink4Burpee.Models.ViewModels
{
    public class DrinkViewModel
    {
        public DateTime CreatedDateTime { get; set; }

        public DateTime? ClosedDateTime { get; set; }

        public string DrinkType { get; set; }

        public int DrinkBurpeeCount { get; set; }

        public int PenaltyBurpeeCount { get; set; }

        public int ExerciseBurpeeCount { get; set; }
    }
}