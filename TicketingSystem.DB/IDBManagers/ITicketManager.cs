using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ITicketManager
    {
        List<Ticket> GetAllTickets(bool isDependeciesToBeLoadedWithTicket = false);
        Ticket GetTicketByID(int id, bool isDependeciesToBeLoadedWithTicket = true);
        Ticket UpsertTicket(Ticket ticket);
    }
}