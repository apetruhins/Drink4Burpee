using Drink4Burpee.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Drink4Burpee.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IApplicationSettings _settings;
        protected readonly IUserService _userService;

        public BaseController(ILogger<BaseController> logger,
            IApplicationSettings settings,
            IUserService userService)
        {
            _logger = logger;
            _settings = settings;
            _userService = userService;
        }
    }
}