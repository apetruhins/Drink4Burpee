using Drink4Burpee.Models;

namespace Drink4Burpee.Services.Base
{
    public abstract class BaseService
    {
        protected readonly IDrink4BurpeeDbSettings _settings;

        public BaseService(IDrink4BurpeeDbSettings settings)
        {
            _settings = settings;
        }
    }
}