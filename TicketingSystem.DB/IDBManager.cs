using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB
{
    public interface IDBManager
    {
        bool UpsertStatus(Status status);
        bool UpsertCategory(Category category);
        bool UpsertTicketType(TicketType type);
        bool UpsertPriority(Priority priority);
        List<Ticket> GetAllTickets();
        bool UpsertSubCategory(SubCategory subCategory);
        List<SubCategory> GetAllSubCategory();
        bool UpsertImpact(Impact impact);
        List<Impact> GetAllImpacts();
        Ticket GetTicketById(int id);
        User GetUserById(int id);

        bool SaveComment(Comment comment);
        bool UploadAttachment(Attachment attachment);
        bool UploadAttachments(List<Attachment> attachments);
        bool UpsertTicketObject(Ticket ticket);
        List<Comment> GetCommentsByTicketId(int ticketId);
        List<ViewModel.History> GetHistoriesByTicketId(int ticketId);
        List<ViewModel.TicketType> GetAllTicketTypes();
        List<ViewModel.Status> GetAllStatus();
        List<ViewModel.Category> GetAllCategories();
        List<ViewModel.Priority> GetAllPriorities();
    }
}