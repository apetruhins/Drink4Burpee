using System.Collections.Generic;
using System.Threading.Tasks;
using Drink4Burpee.Entities;

namespace Drink4Burpee.Services.Interfaces
{
    public interface IDrinkService
    {
        Task<Drink> AddDrinkAsync(User user, Drink drink);

        List<Drink> GetOpenDrinks(User user, int limit, int offset);

        int GetOpenDrinkBurpeesCount(User user);

        List<Drink> GetClosedDrinks(User user, int limit, int offset);
    }
}