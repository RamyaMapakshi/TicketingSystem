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
        public List<Ticket> GetAllTickets(bool isDependeciesToBeLoadedWithTicket = false)
        {
            return CommonDBManager.TickerDbManager.GetAllTickets(isDependeciesToBeLoadedWithTicket);
        }
        public bool UpsertImpact(Impact impact)
        {
            return CommonDBManager.ImpactDBManager.UpsertImpact(impact);
        }
        public List<Impact> GetAllImpacts()
        {
            return CommonDBManager.ImpactDBManager.GetAllImpacts();
        }
        public bool UpsertSubCategory(SubCategory subCategory)
        {
            return CommonDBManager.SubCategoryDBManager.UpsertSubCategory(subCategory);
        }
        public List<SubCategory> GetAllSubCategory()
        {
            return CommonDBManager.SubCategoryDBManager.GetAllsubCategories();
        }
        public int UpsertTicketObject(Ticket ticket)
        {
            foreach (var prop in ticket.GetType().GetProperties())
            {
                if (prop.PropertyType == new User().GetType())
                {
                    var user = (prop.GetValue(ticket) as User);
                    if (user != null)
                    {
                        var dbUser = CommonDBManager.UserManager.GetUserByEmail(user.Email);
                        if (dbUser == null)
                        {
                            prop.SetValue(ticket, CommonDBManager.UserManager.UpsertUser(prop.GetValue(ticket) as User));
                        }
                        else
                        {
                            dbUser.PhoneNumber = user.PhoneNumber;
                            prop.SetValue(ticket, dbUser);
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
                    attachment.UploadedBy = ticket.ModifiedBy;
                    attachment.FileUrl = attachment.FileUrl.Replace("$$ticketId$$", Convert.ToString(ticket.ID));
                    SaveAttachmentDetail(attachment);
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
            return ticket.ID;
        }
        public bool SaveComment(Comment comment)
        {
            if (comment.Attachment != null)
            {
                comment.Attachment.ID = SaveAttachmentDetail(comment.Attachment);
            }
            return CommonDBManager.CommentDbManager.UpsertComment(comment);
        }
        public int SaveAttachmentDetail(Attachment attachment)
        {
            return CommonDBManager.AttachmentDBManager.SaveAttachmentDetail(attachment);
        }
        public List<Attachment> GetAttachmentsDetailByTicketId(int ticketId)
        {
            return CommonDBManager.AttachmentDBManager.GetAttachmentsDetailByTicketId(ticketId);
        }
        public bool SaveAttachmentsDetail(List<Attachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                SaveAttachmentDetail(attachment);
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

        public Ticket GetTicketById(int id, bool isDependeciesToBeLoadedWithTicket)
        {
            return CommonDBManager.TickerDbManager.GetTicketByID(id, isDependeciesToBeLoadedWithTicket);
        }

        public List<Configuration> GetAllConfigurations()
        {
            return CommonDBManager.ConfigurationDBManager.GetAllConfigurations();
        }

        public Configuration GetConfigurationByKey(string key)
        {
            return CommonDBManager.ConfigurationDBManager.GetConfigurationByKey(key);
        }

        public bool UpdateConfiguration(Configuration config)
        {
            return CommonDBManager.ConfigurationDBManager.UpdateConfiguration(config);
        }

        public int CreateTicketViaEmail(Email email)
        {
            ViewModel.Ticket ticket = new Ticket();
            ticket.Title = email.Subject;
            ticket.Description = email.Body;
            ticket.IsTicketGeneratedViaEmail = true;
            //User requestedUser = CommonDBManager.UserManager.GetUserByEmail(email.From.Email);
            //if (requestedUser == null)
            //{
            //    requestedUser = new User()
            //    {
            //        Email = email.From,
            //        ID = 0,
            //        IsActive = true,
            //        IsExternalUser = true
            //    };
            //}
            ticket.RequestedBy = email.From;
            ticket.RequestedFor = email.From;
            ticket.Modified = DateTime.Now;
            ticket.Created = DateTime.Now;
            ticket.ModifiedBy = email.From;
            ticket.CreatedBy = email.From;
            ticket.RequestedFor = null;
            foreach (var cc in email.CC)
            {
                var user = CommonDBManager.UserManager.GetUserByEmail(cc.Email);
                if (user==null)
                {
                    CommonDBManager.UserManager.UpsertUser(cc);
                }
                ticket.EmailsToNotify += cc.Email+";";
            }
            foreach (var to in email.To)
            {
                var user = CommonDBManager.UserManager.GetUserByEmail(to.Email);
                if (user == null)
                {
                    CommonDBManager.UserManager.UpsertUser(to);
                }
                ticket.EmailsToNotify += to.Email+";";
            }
            List<Attachment> _attachments = email.Attachments;
            int index = 0;
            foreach (var attachment in email.Attachments)
            {

                _attachments[index++].ID=SaveAttachmentDetail(attachment);
            }
            ticket.ID= UpsertTicketObject(ticket);
            email.TicketID = ticket.ID;
            email.PreviousEmail = CommonDBManager.EmailDBManager.GetPreviousEmailByTicketId(ticket.ID);
            email.Attachments = _attachments;
            CommonDBManager.EmailDBManager.SaveEmailDetailsInDB(email);
            return ticket.ID;
        }
    }
}
