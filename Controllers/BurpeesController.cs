using System;
using System.Threading.Tasks;
using Drink4Burpee.Controllers.Base;
using Drink4Burpee.Models;
using Drink4Burpee.Models.InputModels;
using Drink4Burpee.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Drink4Burpee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BurpeesController : BaseController
    {
        private readonly IExerciseBurpeeService _exerciseBurpeeService;
        private readonly IDrinkBurpeeService _drinkBurpeeService;

        public BurpeesController(ILogger<BurpeesController> logger,
            IDrink4BurpeeDbSettings settings,
            IUserService userService,
            IExerciseBurpeeService exerciseBurpeeService,
            IDrinkBurpeeService drinkBurpeeService)
            : base(logger, settings, userService)
        {
            _exerciseBurpeeService = exerciseBurpeeService;
            _drinkBurpeeService = drinkBurpeeService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExerciseBurpeeInputModel burpeeModel)
        {
            try
            {
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                await _exerciseBurpeeService.RegisterExerciseBurpeesAsync(user, burpeeModel.Count);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException aoorex)
            {
                var msg = aoorex.Message;
                _logger.LogError(aoorex, msg);
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                var msg = "Failed to register exercise burpees";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetDebtCount()
        {
            try
            {
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                var debtBurpeeCount = _drinkBurpeeService.GetDebtBurpeeCount(user);
                return Ok(debtBurpeeCount);
            }
            catch (Exception ex)
            {
                var msg = "Failed to get debt burpee count";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }
    }
}
