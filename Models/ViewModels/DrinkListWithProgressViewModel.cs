namespace Drink4Burpee.Models.ViewModels
{
    public class DrinkListWithProgressViewModel : DrinkListViewModel
    {
        public ProgressViewModel Progress { get; set; } = new ProgressViewModel();
    }
}