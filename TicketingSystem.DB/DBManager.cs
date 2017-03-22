using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
        private bool IsUsersSame(User user, User dbUser)
        {
            bool isUsersSame = true;

            foreach (var prop in user.GetType().GetProperties())
            {
                if (!prop.GetValue(user).Equals(dbUser.GetType().GetProperties().FirstOrDefault(x => x.Name == prop.Name).GetValue(dbUser)))
                {
                    isUsersSame = false;
                }
            }

            return isUsersSame;
        }
        public int UpsertTicketObject(Ticket ticket)
        {

            bool isNewTicket = ticket.ID == 0 ? true : false;
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
                            prop.SetValue(ticket, IsUsersSame(user, dbUser) ? dbUser : CommonDBManager.UserManager.UpsertUser(user));
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
                    attachment.FileUrl = attachment.FileUrl.Replace("$$ticketId$$", string.Format("{0:000000}", ticket.ID));
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
            if (isNewTicket && ticket.IsTicketGeneratedViaEmail.Value)
                return ticket.ID;
            SendEmail(ticket, isNewTicket);
            return ticket.ID;
        }
        private void SendEmail(Ticket ticket, bool isNewTicket = true)
        {
            var emailTemplate = CommonDBManager.EmailDBManager.GetEmailTemplateByTitle(isNewTicket ? "NewWebTicket" : "");
            Email email = new Email();
            email.Subject = string.Format(emailTemplate.Subject, ticket.ID, ticket.Title);
            email.Body = string.Format(emailTemplate.Body, ticket.RequestedBy.Name, ticket.ID);
            email.To.Add(ticket.RequestedBy);
            email.From = new User() { Email = "technoverttest2@kci.com" };
            foreach (var ccEmail in ticket.EmailsToNotify.Split(';'))
            {
                email.CC.Add(new User() { Email = ccEmail });
            }
            CommonDBManager.EmailDBManager.SendEmail(email);
        }
        public bool SaveComment(Comment comment)
        {
            if (comment.Attachments != null)
            {
                List<Attachment> _attachments = new List<Attachment>();
                foreach (var attachment in comment.Attachments)
                {
                    attachment.ID = SaveAttachmentDetail(attachment);
                    _attachments.Add(attachment);
                }
                comment.Attachments = _attachments;
            }
            return CommonDBManager.CommentDbManager.UpsertComment(comment);
        }
        public List<ViewModel.TicketTemplate> GetAllTicketTemplates()
        {
            return CommonDBManager.TickerDbManager.GetAllTicketTemplates();
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
        private string ExtractText(string html)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            string s = reg.Replace(html, " ");
            s = HttpUtility.HtmlDecode(s);
            return s;
        }
        private int CreateTicketViaEmail(Email email)
        {
            ViewModel.Ticket ticket = new ViewModel.Ticket();
            ticket.Title = email.Subject;
            ticket.Description = ExtractText(email.Body);
            ticket.IsTicketGeneratedViaEmail = true;
            ticket.RequestedBy = email.From;
            ticket.RequestedFor = email.From;
            ticket.Modified = DateTime.Now;
            ticket.Created = DateTime.Now;
            ticket.ModifiedBy = email.From;
            ticket.CreatedBy = email.From;
            foreach (var cc in email.CC)
            {
                var user = CommonDBManager.UserManager.GetUserByEmail(cc.Email);
                if (user == null)
                {
                    CommonDBManager.UserManager.UpsertUser(cc);
                }
                ticket.EmailsToNotify += cc.Email + ";";
            }
            foreach (var to in email.To)
            {
                var user = CommonDBManager.UserManager.GetUserByEmail(to.Email);
                if (user == null)
                {
                    CommonDBManager.UserManager.UpsertUser(to);
                }
                ticket.EmailsToNotify += to.Email + ";";
            }

            return UpsertTicketObject(ticket);
        }
        public ViewModel.Email UpsertTicketViaEmail(Email email)
        {
            int ticketId = 0;
            bool isNewTicket = true;
            List<Attachment> _attachments = new List<Attachment>();

            if (!CommonDBManager.EmailParserDBManager.CheckIfValidEmail(email))
            {
                ticketId = CommonDBManager.EmailParserDBManager.CheckIfEmailIsForAlreadyCreatedTicketFromSubjectAndReturnTicketId(email.Subject);
                if (ticketId != 0)
                {
                    User user = CommonDBManager.UserManager.GetUserByEmail(email.From.Email);
                    if (user == null)
                    {
                        user = CommonDBManager.UserManager.UpsertUser(email.From);
                    }
                    foreach (var attachment in email.Attachments)
                    {
                        attachment.UploadedBy = user;
                        attachment.TicketId = ticketId;
                        attachment.ID = SaveAttachmentDetail(attachment);
                        _attachments.Add(attachment);
                    }
                    email.Attachments = _attachments;
                    SaveComment(new Comment()
                    {
                        ID = 0,
                        Created = DateTime.Now,
                        CreatedBy = user,
                        ModifiedBy = user,
                        Modified = DateTime.Now,
                        IsPrivate = false,
                        TicketId = ticketId,
                        Details = ExtractText(email.Body),
                        Attachments = email.Attachments
                    });
                    isNewTicket = false;
                }
            }
            else
            {
                ticketId = CreateTicketViaEmail(email);
                foreach (var attachment in email.Attachments)
                {
                    attachment.UploadedBy = CommonDBManager.UserManager.GetUserByEmail(email.From.Email);
                    attachment.TicketId = ticketId;
                    attachment.ID = SaveAttachmentDetail(attachment);
                    _attachments.Add(attachment);
                }
                email.Attachments = _attachments;
            }
            if (ticketId == 0)
            {
                return null;
            }

            email.TicketID = ticketId;

            email.PreviousEmail = CommonDBManager.EmailDBManager.GetPreviousEmailByTicketId(ticketId);
            CommonDBManager.EmailDBManager.SaveEmailDetailsInDB(email);

            if (isNewTicket)
            {
                email.Body = string.Format("Hi {0},<br/><br/>Ticket created with ID {1}.<br/>Please follow/reply to this thread for further assistance.<br/><br/>Thanks,<br/>Support Team", email.From.Name, string.Format("TKT{0:000000}", ticketId));
                email.Subject = string.Format("RE: {1} -TKT{0:000000}", ticketId, email.Subject);
            }
            else
            {
                email.Body = "Thank You.<br/> Your response has been saved.";
            }
            return email;
        }
    }
}
