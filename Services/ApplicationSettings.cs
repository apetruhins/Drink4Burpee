using Drink4Burpee.Services.Interfaces;

namespace Drink4Burpee.Services
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string UsersCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string DefaultUserId { get; set; }
        public string GuiBaseURL { get; set; }
        
        public bool AddPenaltyOnRequest { get; set; }
    }
}