using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ITicketManager
    {
        List<Ticket> GetAllTickets();
        Ticket GetTicketByID(int id);
        Ticket UpsertTicket(Ticket ticket);
    }
}