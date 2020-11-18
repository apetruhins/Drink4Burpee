using System;
using System.Collections.Generic;
using Drink4Burpee.Entities.Enums;

namespace Drink4Burpee.Entities
{
    public class Drink
    {
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DrinkType DrinkType { get; set; }

        public DateTime? ClosedDateTime { get; set; }

        public List<DrinkBurpee> DrinkBurpees { get; set; } = new List<DrinkBurpee>();

        public List<ExerciseBurpee> ExerciseBurpees { get; set; } = new List<ExerciseBurpee>();

        public bool IsClosed => ClosedDateTime.HasValue;
    }
}