using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB
{
    public interface IDBManager
    {
        List<Ticket> GetAllTickets();
        User GetUserById(int id);
        bool SaveComment(Comment comment);
        bool UploadAttachment(Attachment attachment);
        bool UploadAttachments(List<Attachment> attachments);
        bool UpsertTicketObject(Ticket ticket);
        bool UpsertStatus(Status status);
    }
}