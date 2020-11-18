using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drink4Burpee.Constants;
using Drink4Burpee.Entities;
using Drink4Burpee.Entities.Enums;
using Drink4Burpee.Models;
using Drink4Burpee.Services.Base;
using Drink4Burpee.Services.Interfaces;

namespace Drink4Burpee.Services
{
    public class DrinkBurpeeService : BaseService, IDrinkBurpeeService
    {
        private readonly IUserService _userService;

        public DrinkBurpeeService(IDrink4BurpeeDbSettings settings,
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
            var penaltyDrink =  GetPenaltyDrink(user);

            if (penaltyDrink == null)
            {
                return;
            }

            var hasPenaltyBurpeeLast24H = user
                .Drinks
                .SelectMany(drink => drink.DrinkBurpees)
                .Any(b => b.BurpeeType == DrinkBurpeeType.Penalty 
                    && b.CreatedDateTime.AddDays(1) > DateTime.Now);

            if (hasPenaltyBurpeeLast24H)
            {
                return;
            }

            var penaltyBurpee = new DrinkBurpee
            {
                BurpeeType = DrinkBurpeeType.Penalty,
                Count = BusinessConstants.DRINK_BURPEE_COUNT_PENALTY + user.Level - 1
            };

            penaltyDrink.DrinkBurpees.Add(penaltyBurpee);
            await _userService.UpdateUserAsync(user);
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
    }
}