using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IPriorityManager
    {
        bool UpsertCategory(Priority priority);
    }
}