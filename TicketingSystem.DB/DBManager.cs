using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.DBManagers;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB
{
    public class DBManager : IDBManager
    {
        
        public bool UpsertStatus(Status status)
        {
            return CommonDBManager.StatusDBManager.UpsertStatus(status);
        }
        public bool UpsertCategory(Category category)
        {
            return CommonDBManager.CategoryDBManager.UpsertCategory(category);
        }
        public bool UpsertTicketType(TicketType type)
        {
            return CommonDBManager.TicketTypeDBManager.UpsertTicketType(type);
        }
        public bool UpsertPriority(Priority priority)
        {
            return CommonDBManager.PriorityDBManager.UpsertPriority(priority);
        }
        public List<Ticket> GetAllTickets()
        {
            return CommonDBManager.TickerDbManager.GetAllTickets();
        }
        public bool UpsertTicketObject(Ticket ticket)
        {
            foreach (var prop in ticket.GetType().GetProperties())
            {
                if (prop.PropertyType == new User().GetType())
                {
                    var user = (prop.GetValue(ticket) as User);
                    if (user != null)
                    {
                        user = CommonDBManager.UserManager.GetUserByEmail(user.Email);
                        if ( user== null)
                        {
                            prop.SetValue(ticket, CommonDBManager.UserManager.UpsertUser(prop.GetValue(ticket) as User));
                        }
                        else
                        {
                            prop.SetValue(ticket, user);
                        }
                    }
                }
            }
            ticket = CommonDBManager.TickerDbManager.UpsertTicket(ticket);
            if (ticket.Attachments.Where(x => x.ID == 0).ToList().Count > 0)
            {
                foreach (var attachment in ticket.Attachments.Where(x => x.ID == 0))
                {
                    attachment.TicketId = ticket.ID;
                    UploadAttachment(attachment);
                }
            }
            if (ticket.Comments.Where(x => x.ID == 0).ToList().Count > 0)
            {
                foreach (var comment in ticket.Comments)
                {
                    comment.TicketId = ticket.ID;
                    SaveComment(comment);
                }
            }
            return true;
        }
        public bool SaveComment(Comment comment)
        {
            if (comment.Attachment != null)
            {
                UploadAttachment(comment.Attachment);
            }
            return CommonDBManager.CommentDbManager.UpsertComment(comment);
        }
        public bool UploadAttachment(Attachment attachment)
        {
            return CommonDBManager.AttachmentDBManager.UpsertAttachment(attachment);
        }
        public bool UploadAttachments(List<Attachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                UploadAttachment(attachment);
            }
            return true;
        }
        public User GetUserById(int id)
        {
            return CommonDBManager.UserManager.GetUserById(id);
        }
        public List<Comment> GetCommentsByTicketId(int ticketId)
        {
            return CommonDBManager.CommentDbManager.GetCommentsByTicketId(ticketId);
        }
        public List<History> GetHistoriesByTicketId(int ticketId)
        {
            return CommonDBManager.HistoryDBManager.GetHistoriesByTicketId(ticketId);
        }

        public List<TicketType> GetAllTicketTypes()
        {
            return CommonDBManager.TicketTypeDBManager.GetAllTicketTypes();
        }

        public List<Status> GetAllStatus()
        {
            return CommonDBManager.StatusDBManager.GetAllStatus();
        }

        public List<Category> GetAllCategories()
        {
            return CommonDBManager.CategoryDBManager.GetAllCategories();
        }

        public List<Priority> GetAllPriorities()
        {
            return CommonDBManager.PriorityDBManager.GetAllPriorities();
        }
    }
}
