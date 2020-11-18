using System.Threading.Tasks;
using Drink4Burpee.Entities;

namespace Drink4Burpee.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string id);

        Task UpdateUserAsync(User user);

        Task UpdateUserLevelAsync(User user);
    }
}