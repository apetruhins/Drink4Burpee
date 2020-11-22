namespace Drink4Burpee.Services.Interfaces
{
    public interface IApplicationSettings
    {
        string UsersCollectionName { get; set; }
        
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string DefaultUserId { get; set; }

        string GuiBaseURL { get; set; }

        bool AddPenaltyOnRequest{ get; set; }
    }
}