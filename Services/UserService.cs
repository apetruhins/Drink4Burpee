using System;
using System.Linq;
using System.Threading.Tasks;
using Drink4Burpee.Constants;
using Drink4Burpee.Entities;
using Drink4Burpee.Models;
using Drink4Burpee.Services.Base;
using Drink4Burpee.Services.Interfaces;
using MongoDB.Driver;

namespace Drink4Burpee.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IDrink4BurpeeDbSettings settings)
            : base(settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _users.Find<User>(user => user.Id == id)
                .SingleAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task UpdateUserLevelAsync(User user)
        {
            var nextLevel = user.Level + 1;
            var nextLevelFibonacciNumber = GetNextLevelFibonacciNumber(nextLevel);
            var nextLevelBurpeeCount = nextLevelFibonacciNumber * BusinessConstants.BURPEE_BASE_COUNT_IN_LEVEL;

            var exerciseBurpeeCount = user.Drinks
                .SelectMany(drink => drink.ExerciseBurpees)
                .Sum(eb => eb.Count);

            if (exerciseBurpeeCount >= nextLevelBurpeeCount)
            {
                user.Level = nextLevel;
            }

            await UpdateUserAsync(user);
        }

        private int GetNextLevelFibonacciNumber(int nextLevel)
        {
            if (nextLevel < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(nextLevel));
            }

            var n1 = 0;
            var n2 = 1;
            var fibonacciNumber = 1;

            for (int i = 1; i < nextLevel; i++)
            {
                fibonacciNumber = GetNextFibonacciNumber(n1, n2);
                n1 = n2;
                n2 = fibonacciNumber;
            }

            return fibonacciNumber;
        }

        private int GetNextFibonacciNumber(int f1, int f2) => f1 + f2;
    }
}