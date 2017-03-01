using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IHistoryManager
    {
        void ComapreChangesInTicket(Ticket ticket, Ticket oldTicket);
        bool CreateNewAttachmentHistory(Attachment attachment);
        bool CreateNewCommentHistory(Comment comment);
        bool CreateTicketHistory(Ticket ticket);
        List<History> GetChnagedValueHistory(Ticket newTicket, Ticket oldTicket);
        List<History> GetHistoriesByTicketId(int ticketId);
        History GetLastHistoryByTicketId(int ticketId);
        bool UpsertHistory(History history);
    }
}