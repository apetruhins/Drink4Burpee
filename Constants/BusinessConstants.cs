using System.Collections.Generic;
using Drink4Burpee.Entities.Enums;

namespace Drink4Burpee.Constants
{
    public static class BusinessConstants
    {
        public const int DRINK_BURPEE_COUNT_PENALTY = 10;

        public const int DRINK_BURPEE_COUNT_DOUBLE_RATE_MULTIPLIER = 2;

        public const int DRINK_COUNT_LIMIT_MAX = 200;

        public const int BURPEE_BASE_COUNT_IN_LEVEL = 1000;

        public static readonly Dictionary<DrinkType, int> DefaultDrinkBurpeeCount = new Dictionary<DrinkType, int>
        {
            { DrinkType.GlassOfWine, 10 },
            { DrinkType.PintOfLightBeer, 10 },
            { DrinkType.Glass330LightBeer, 7 },
            { DrinkType.Glass330StrongBeer, 10 },
            { DrinkType.PintOfStrongBeer, 15 }
        };
    }
}