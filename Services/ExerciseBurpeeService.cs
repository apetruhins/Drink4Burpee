using System;
using System.Linq;
using System.Threading.Tasks;
using Drink4Burpee.Entities;
using Drink4Burpee.Services.Base;
using Drink4Burpee.Services.Interfaces;

namespace Drink4Burpee.Services
{
    public class ExerciseBurpeeService : BaseService, IExerciseBurpeeService
    {
        private readonly IUserService _userService;

        public ExerciseBurpeeService(IApplicationSettings settings,
            IUserService userService)
            : base(settings)
        {
            _userService = userService;
        }

        public async Task RegisterExerciseBurpeesAsync(User user, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var drinks = user.Drinks
                .Where(d => !d.IsClosed)
                .OrderBy(d => d.CreatedDateTime)
                .ToList();

            if (drinks.Count == 0)
            {
                var lastDrink = user.Drinks
                    .OrderByDescending(d => d.CreatedDateTime)
                    .FirstOrDefault();

                if (lastDrink != null)
                {
                    lastDrink.ExerciseBurpees.Add(
                        new ExerciseBurpee
                        {
                            Count = count
                        });
                }
            }
            else
            {
                for (int i = 0; i < drinks.Count; i++)
                {
                    if (count <= 0)
                    {
                        break;
                    }

                    var drink = drinks[i];
                    var drinkBurpees = drink.DrinkBurpees.Sum(b => b.Count);
                    var exerciseBurpees = drink.ExerciseBurpees.Sum(b => b.Count);
                    var burpeesToAdd = drinkBurpees - exerciseBurpees;

                    if (burpeesToAdd <= 0)
                    {
                        drink.ClosedDateTime = DateTime.Now;
                        continue;
                    }

                    if (burpeesToAdd > count)
                    {
                        burpeesToAdd = count;
                    }
                    
                    count -= burpeesToAdd;

                    var burpee = new ExerciseBurpee
                    {
                        Count = burpeesToAdd
                    };

                    if (i == (drinks.Count - 1) && count > 0)
                    {
                        burpee.Count += count;
                        count = 0;
                    }

                    if ((burpee.Count + exerciseBurpees) >= drinkBurpees)
                    {
                        drink.ClosedDateTime = DateTime.Now;
                    }

                    drink.ExerciseBurpees.Add(burpee);
                }
            }

            await _userService.UpdateUserAsync(user);
            await _userService.UpdateUserLevelAsync(user);
        }
    }
}