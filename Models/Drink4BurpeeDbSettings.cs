namespace Drink4Burpee.Models
{
    public class Drink4BurpeeDbSettings : IDrink4BurpeeDbSettings
    {
        public string UsersCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string DefaultUserId { get; set; }
    }

    public interface IDrink4BurpeeDbSettings
    {
        string UsersCollectionName { get; set; }
        
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string DefaultUserId { get; set; }
    }
}