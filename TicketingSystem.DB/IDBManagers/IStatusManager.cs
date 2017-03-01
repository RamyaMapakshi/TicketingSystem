using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IStatusManager
    {
        bool UpsertStatus(Status status);
    }
}