using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drink4Burpee.Constants;
using Drink4Burpee.Entities;
using Drink4Burpee.Models;
using Drink4Burpee.Services.Base;
using Drink4Burpee.Services.Interfaces;

namespace Drink4Burpee.Services
{
    public class DrinkService : BaseService, IDrinkService
    {
        private readonly IUserService _userService;

        private readonly IDrinkBurpeeService _drinkBurpeeService;

        public DrinkService(IDrink4BurpeeDbSettings settings,
            IUserService userService,
            IDrinkBurpeeService drinkBurpeeService)
            : base(settings)
        {
            _userService = userService;
            _drinkBurpeeService = drinkBurpeeService;
        }

        public async Task<Drink> AddDrinkAsync(User user, Drink drink)
        {
            user.Drinks.Add(drink);
            drink.DrinkBurpees = _drinkBurpeeService.GetBurpeesForDrink(user, drink);
            await _userService.UpdateUserAsync(user);
            return drink;
        }

        public List<Drink> GetOpenDrinks(User user, int limit, int offset)
        {
            if (limit <= 0 || limit > BusinessConstants.DRINK_COUNT_LIMIT_MAX + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(limit));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return user.Drinks
                .Where(d => !d.IsClosed)
                .OrderBy(d => d.CreatedDateTime)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        public int GetOpenDrinkBurpeesCount(User user)
        {
            var openedDrinks = user.Drinks
                .Where(d => d.IsClosed == false)
                .ToList();

            var drinkBurpeeCount = openedDrinks.SelectMany(d => d.DrinkBurpees).Sum(db => db.Count);
            var exerciseBurpeeCount = openedDrinks.SelectMany(d => d.ExerciseBurpees).Sum(eb => eb.Count);
            var result = drinkBurpeeCount - exerciseBurpeeCount;

            return result < 0 ? 0 : result;
        }

        public List<Drink> GetClosedDrinks(User user, int limit, int offset)
        {
            if (limit <= 0 || limit > BusinessConstants.DRINK_COUNT_LIMIT_MAX + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(limit));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return user.Drinks
                .Where(d => d.IsClosed)
                .OrderByDescending(d => d.CreatedDateTime)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }
    }
}