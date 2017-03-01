using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ITicketTypeManager
    {
        bool UpsertCategory(TicketType ticketType);
    }
}