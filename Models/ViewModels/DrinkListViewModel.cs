using System.Collections.Generic;

namespace Drink4Burpee.Models.ViewModels
{
    public class DrinkListViewModel
    {
        public List<DrinkViewModel> Drinks { get; set; } = new List<DrinkViewModel>();

        public int TotalBurpees { get; set; }
        
        public int Offset {get; set; } = 0;

        public bool HasMore { get; set; } = false;
    }
}