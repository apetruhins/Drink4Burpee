using Drink4Burpee.Models;
using Drink4Burpee.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Drink4Burpee.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IDrink4BurpeeDbSettings _settings;
        protected readonly IUserService _userService;

        public BaseController(ILogger<BurpeesController> logger,
            IDrink4BurpeeDbSettings settings,
            IUserService userService)
        {
            _logger = logger;
            _settings = settings;
            _userService = userService;
        }
    }
}