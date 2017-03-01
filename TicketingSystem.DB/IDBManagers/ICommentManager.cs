using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ICommentManager
    {
        List<Comment> GetCommentsByTicketId(int ticketId);
        bool UpsertComment(Comment comment);
    }
}