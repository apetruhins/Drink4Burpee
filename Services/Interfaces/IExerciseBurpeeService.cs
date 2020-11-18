using System.Threading.Tasks;
using Drink4Burpee.Entities;

namespace Drink4Burpee.Services.Interfaces
{
    public interface IExerciseBurpeeService
    {
        Task RegisterExerciseBurpeesAsync(User user, int count);
    }
}