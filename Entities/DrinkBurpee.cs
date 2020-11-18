using Drink4Burpee.Entities.Base;
using Drink4Burpee.Entities.Enums;

namespace Drink4Burpee.Entities
{
    public class DrinkBurpee : BaseBurpee
    {
        public DrinkBurpeeType BurpeeType { get; set; } = DrinkBurpeeType.Normal;
    }
}