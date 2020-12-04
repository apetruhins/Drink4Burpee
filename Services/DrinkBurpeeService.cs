using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drink4Burpee.Constants;
using Drink4Burpee.Entities;
using Drink4Burpee.Entities.Enums;
using Drink4Burpee.Services.Base;
using Drink4Burpee.Services.Interfaces;

namespace Drink4Burpee.Services
{
    public class DrinkBurpeeService : BaseService, IDrinkBurpeeService
    {
        private readonly IUserService _userService;

        public DrinkBurpeeService(IApplicationSettings settings,
            IUserService userService)
            : base(settings)
        {
            _userService = userService;
        }

        public List<DrinkBurpee> GetBurpeesForDrink(User user, Drink drink)
        {
            var burpee = GetDrinkBurpee(user, drink);
            var result = new List<DrinkBurpee>();
            
            result.Add(burpee);

            return result;
        }

        public DrinkBurpee GetDrinkBurpee(User user, Drink drink)
        {
            var result = new DrinkBurpee
            {
                Count = BusinessConstants.DefaultDrinkBurpeeCount[drink.DrinkType] + user.Level - 1
            };

            var penaltyDrink =  GetPenaltyDrink(user);

            if (penaltyDrink != null)
            {
                result.Count *= BusinessConstants.DRINK_BURPEE_COUNT_DOUBLE_RATE_MULTIPLIER;
                result.BurpeeType = DrinkBurpeeType.DoubleRate;
            }

            return result;
        }

        public async Task AddPenaltyDrinkBurpeesAsync(User user)
        {
            var firstPenaltyDrink = GetPenaltyDrink(user);

            if (firstPenaltyDrink == null)
            {
                return;
            }

            var lastPenaltyBurpee = firstPenaltyDrink
                .DrinkBurpees
                .Where(db => db.BurpeeType == DrinkBurpeeType.Penalty)
                .OrderByDescending(db => db.CreatedDateTime)
                .FirstOrDefault();

            if (lastPenaltyBurpee == null)
            {
                lastPenaltyBurpee = user.Drinks
                    .SelectMany(d => d.DrinkBurpees)
                    .Where(db => db.BurpeeType == DrinkBurpeeType.Penalty && db.CreatedDateTime.Date > firstPenaltyDrink.CreatedDateTime.Date)
                    .OrderByDescending(db => db.CreatedDateTime)
                    .FirstOrDefault();
            }

            var hasChanges = false;

            while (true)
            {
                if (lastPenaltyBurpee == null)
                {
                    lastPenaltyBurpee = new DrinkBurpee
                    {
                        BurpeeType = DrinkBurpeeType.Penalty,
                        Count = BusinessConstants.DRINK_BURPEE_COUNT_PENALTY + user.Level - 1,
                        CreatedDateTime = firstPenaltyDrink.CreatedDateTime.AddDays(1)
                    };
                }
                else
                {
                    lastPenaltyBurpee = new DrinkBurpee
                    {
                        BurpeeType = DrinkBurpeeType.Penalty,
                        Count = BusinessConstants.DRINK_BURPEE_COUNT_PENALTY + user.Level - 1,
                        CreatedDateTime = lastPenaltyBurpee.CreatedDateTime.AddDays(1).Date.Add(
                            firstPenaltyDrink.CreatedDateTime.TimeOfDay
                        )
                    };
                }

                if (lastPenaltyBurpee.CreatedDateTime > DateTime.Now)
                {
                    break;
                }

                firstPenaltyDrink.DrinkBurpees.Add(lastPenaltyBurpee);
                hasChanges = true;
            }
            
            if (hasChanges)
            {
                await _userService.UpdateUserAsync(user);
            }
        }

        private Drink GetPenaltyDrink(User user)
        {
            return user.Drinks
                .Where(d => !d.IsClosed && d.CreatedDateTime.AddDays(1) <= DateTime.Now)
                .OrderBy(d => d.CreatedDateTime)
                .FirstOrDefault();
        }

        public int GetDebtBurpeeCount(User user)
        {
            return user.Drinks
                .Where(drink => !drink.IsClosed)
                .SelectMany(drink => drink.DrinkBurpees)
                .Sum(db => db.Count);
        }

        public int GetOpenDrinkBurpeesCount(User user)
        {
            var openedDrinks = user.Drinks
                .Where(d => !d.IsClosed)
                .ToList();

            var drinkBurpeeCount = openedDrinks.SelectMany(d => d.DrinkBurpees).Sum(db => db.Count);
            var exerciseBurpeeCount = openedDrinks.SelectMany(d => d.ExerciseBurpees).Sum(eb => eb.Count);
            var result = drinkBurpeeCount - exerciseBurpeeCount;

            return result < 0 ? 0 : result;
        }
    }
}