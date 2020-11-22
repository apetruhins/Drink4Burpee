using Drink4Burpee.Services.Interfaces;

namespace Drink4Burpee.Services.Base
{
    public abstract class BaseService
    {
        protected readonly IApplicationSettings _settings;

        public BaseService(IApplicationSettings settings)
        {
            _settings = settings;
        }
    }
}