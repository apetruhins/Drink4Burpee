using System.Collections.Generic;
using System.Threading.Tasks;
using Drink4Burpee.Entities;

namespace Drink4Burpee.Services.Interfaces
{
    public interface IDrinkBurpeeService
    {
        List<DrinkBurpee> GetBurpeesForDrink(User user, Drink drink);

        DrinkBurpee GetDrinkBurpee(User user, Drink drink);

        Task AddPenaltyDrinkBurpeesAsync(User user);

        int GetDebtBurpeeCount(User user);

        int GetOpenDrinkBurpeesCount(User user);
    }
}