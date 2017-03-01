using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ITicketTypeManager
    {
        bool UpsertTicketType(TicketType ticketType);
    }
}