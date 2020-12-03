using System;
using System.Threading.Tasks;
using AutoMapper;
using Drink4Burpee.Constants;
using Drink4Burpee.Entities;
using Drink4Burpee.Models.InputModels;
using Drink4Burpee.Services.Interfaces;
using Drink4Burpee.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Drink4Burpee.Controllers.Base;

namespace Drink4Burpee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrinksController :  BaseController
    {
        private readonly IMapper _mapper;
        private readonly IDrinkService _drinkService;
        private readonly IDrinkBurpeeService _drinkBurpeeService;
        private readonly IExerciseBurpeeService _exerciseBurpeeService;

        public DrinksController(ILogger<DrinksController> logger,
            IApplicationSettings settings,
            IUserService userService,
            IMapper mapper, 
            IDrinkService drinkService,
            IDrinkBurpeeService drinkBurpeeService,
            IExerciseBurpeeService exerciseBurpeeService)
            : base(logger, settings, userService)
        {
            _mapper = mapper;
            _drinkService = drinkService;
            _drinkBurpeeService = drinkBurpeeService;
            _exerciseBurpeeService = exerciseBurpeeService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DrinkInputModel drinkModel)
        {
            try
            {
                var drink = _mapper.Map<Drink>(drinkModel);
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                drink = await _drinkService.AddDrinkAsync(user, drink);

                var drinkViewModel = _mapper.Map<DrinkViewModel>(drink);

                return Ok(drinkViewModel);
            }
            catch (AutoMapperMappingException ammex)
            {
                var msg = "Drink type is not supported";
                _logger.LogError(ammex, msg);
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                var msg = "Failed to create new drink";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int limit = ViewConstants.DRINK_COUNT_ON_PAGE_DEFAULT, int offset = 0)
        {
            try
            {
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                var drinks = _drinkService.GetOpenDrinks(user, limit + 1, offset);

                var drinksViewModel = _mapper.Map<DrinkListViewModel>(drinks);

                ApplyPagingOnDrinksViewModel(drinksViewModel, limit, offset);
                
                drinksViewModel.TotalBurpees = _drinkBurpeeService.GetOpenDrinkBurpeesCount(user);

                return Ok(drinksViewModel);
            }
            catch (ArgumentOutOfRangeException aoorex)
            {
                var msg = aoorex.Message;
                _logger.LogError(aoorex, msg);
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                var msg = "Failed to get drink list";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpGet("burpees/count")]
        public async Task<IActionResult> GetBurpeeCountForDrink([FromQuery] string drinkType)
        {
            try
            {
                var drinkModel = new DrinkInputModel { DrinkType = drinkType };
                var drink = _mapper.Map<Drink>(drinkModel);
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                var drinkBurpee = _drinkBurpeeService.GetDrinkBurpee(user, drink);
                return Ok(drinkBurpee.Count);
            }
            catch (AutoMapperMappingException ammex)
            {
                var msg = "Drink type is not supported";
                _logger.LogError(ammex, msg);
                return BadRequest(msg);
            }
            catch(Exception ex)
            {
                var msg = "Failed to get burpee count for drink";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetClosed(int limit = ViewConstants.DRINK_COUNT_ON_PAGE_DEFAULT, int offset = 0)
        {
            try
            {
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                var drinks = _drinkService.GetClosedDrinks(user, limit + 1, offset);

                var drinksViewModel = _mapper.Map<DrinkListViewModel>(drinks);
                
                ApplyPagingOnDrinksViewModel(drinksViewModel, limit, offset);
                
                drinksViewModel.TotalBurpees = _exerciseBurpeeService.GetClosedExerciseBurpeesCount(user);

                return Ok(drinksViewModel);
            }
            catch (ArgumentOutOfRangeException aoorex)
            {
                var msg = aoorex.Message;
                _logger.LogError(aoorex, msg);
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                var msg = "Failed to get drink history";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpOptions("burpees/penalty")]
        public async Task<IActionResult> AddPenaltyDrinkBurpees()
        {
            try
            {
                var user = await _userService.GetUserAsync(_settings.DefaultUserId);
                await _drinkBurpeeService.AddPenaltyDrinkBurpeesAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                var msg = "Failed to calculate penalty burpees";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        private void ApplyPagingOnDrinksViewModel(DrinkListViewModel drinksViewModel, int limit, int offset)
        {
            if (drinksViewModel == null)
            {
                return;
            }

            if (drinksViewModel.Drinks?.Count > limit)
            {
                drinksViewModel.Drinks.RemoveAt(drinksViewModel.Drinks.Count - 1);
                drinksViewModel.HasMore = true;
            }
            else
            {
                drinksViewModel.HasMore = false;
            }

            drinksViewModel.Offset = offset + limit;
        }
    }
}
