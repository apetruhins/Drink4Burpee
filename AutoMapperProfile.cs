using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Drink4Burpee.Entities;
using Drink4Burpee.Entities.Enums;
using Drink4Burpee.Models.InputModels;
using Drink4Burpee.Models.ViewModels;

namespace Drink4Burpee
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateDrinkInputModelToDrinkMap();
            
            CreateDrinkToDrinkViewModelMap();
            CreateDrinkListToDrinkListViewModelMap();
        }

        private void CreateDrinkInputModelToDrinkMap()
        {
            CreateMap<DrinkInputModel, Drink>()
                .ForMember(drink => drink.DrinkType, opt => opt.MapFrom(src => Enum.Parse(typeof(DrinkType), src.DrinkType)));
        }

        private void CreateDrinkToDrinkViewModelMap()
        {
            CreateMap<Drink, DrinkViewModel>()
                .ForMember(vm => vm.ClosedDateTime, opt => opt.MapFrom(src => src.ClosedDateTime))
                .ForMember(vm => vm.DrinkType, opt => opt.MapFrom(src => src.DrinkType.ToString()))
                .ForMember(vm => vm.DrinkBurpeeCount, opt => opt.MapFrom(src => src.DrinkBurpees.Where(db => db.BurpeeType != DrinkBurpeeType.Penalty).Sum(db => db.Count)))
                .ForMember(vm => vm.PenaltyBurpeeCount, opt => opt.MapFrom(src => src.DrinkBurpees.Where(db => db.BurpeeType == DrinkBurpeeType.Penalty).Sum(db => db.Count)))
                .ForMember(vm => vm.ExerciseBurpeeCount, opt => opt.MapFrom(src => src.ExerciseBurpees.Sum(eb => eb.Count)));
        }

        private void CreateDrinkListToDrinkListViewModelMap()
        {
            CreateMap<IEnumerable<Drink>, DrinkListViewModel>()
                .ForMember(vm => vm.Drinks, opt => opt.MapFrom(src => src));
        }
    }
}