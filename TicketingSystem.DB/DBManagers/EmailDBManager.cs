using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.DBManagers
{
    public class EmailDBManager : IEmailDBManager
    {
        public int GetPreviousEmailByTicketId(int ticketId)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                var _email = context.Emails.Where(x => x.TicketID == ticketId).OrderByDescending(x => x.ID).FirstOrDefault();
                if (_email == null)
                {
                    return 0;
                }
                return _email.ID;
            }
        }

        public bool SaveEmailDetailsInDB(Email email)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Email emailObjToBeUpdated = new Database.Email();
                emailObjToBeUpdated.Body = email.Body;
                emailObjToBeUpdated.DateTimeCreated = email.DateTimeCreated;
                emailObjToBeUpdated.DateTimeReceived = email.DateTimeReceived;
                emailObjToBeUpdated.DateTimeSent = email.DateTimeSent;
                emailObjToBeUpdated.From = email.From.Email;
                emailObjToBeUpdated.TicketID = email.TicketID;
                emailObjToBeUpdated.Subject = email.Subject;
                emailObjToBeUpdated.PreviousEmail = email.PreviousEmail;
                emailObjToBeUpdated.ID = email.ID;
                foreach (var attachment in email.Attachments)
                {
                    emailObjToBeUpdated.AttachmentIds += attachment.ID + ",";
                }
                foreach (var to in email.To)
                {
                    emailObjToBeUpdated.To += to.Email + ",";
                }
                foreach (var cc in email.CC)
                {
                    emailObjToBeUpdated.CC += cc.Email + ",";
                }
                context.Emails.Add(emailObjToBeUpdated);
                context.SaveChanges();
                return true;
            }
        }

        public bool SendEmail(Email email)
        {
            MailMessage message = new MailMessage();
            foreach (var to in email.To)
            {
                if (!string.IsNullOrEmpty(to.Email))
                {
                    message.To.Add(to.Email);
                }
            }
            message.From = new MailAddress(email.From.Email);
            message.Body = "<style>div,table{font-family: Segoe UI,Frutiger,Frutiger Linotype,Dejavu Sans,Helvetica Neue,Arial,sans-serif;}</style>" + email.Body;
            message.IsBodyHtml = true;
            message.Subject = email.Subject;
            foreach (var cc in email.CC)
            {
                if (!string.IsNullOrEmpty(cc.Email))
                {
                    message.CC.Add(cc.Email);
                }
            }
            using (SmtpClient smtp = new SmtpClient("kci-coloca01.corp.kci.com", 25))
            {
                try
                {
                    smtp.Credentials = new NetworkCredential("technoverttest2@kci.com", "1qaz2wsx");
                    smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    //ex
                    return false;
                }
            }
        }
        public List<ViewModel.EmailTemplate> GetAllEmailTemplates()
        {
            List<ViewModel.EmailTemplate> emailTempates = new List<EmailTemplate>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var template in context.EmailTemplates)
                {
                    emailTempates.Add(ConvertToViewModelObject(template));
                }
            }
            return emailTempates;
        }
        public ViewModel.EmailTemplate GetEmailTemplateByTitle(string title)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                return ConvertToViewModelObject(context.EmailTemplates.FirstOrDefault(x => x.Title == title));
            }
        }
        private ViewModel.EmailTemplate ConvertToViewModelObject(Database.EmailTemplate template)
        {
            if (template == null)
            {
                return null;
            }
            return new ViewModel.EmailTemplate()
            {
                BCC = template.BCC,
                Body = template.Body,
                CC = template.CC,
                ID = template.ID,
                IsActive = template.IsActive,
                Subject = template.Subject,
                Title = template.Title
            };
        }
        private ViewModel.Email ConvertToViewModelObject(Database.Email email)
        {
            if (email == null)
            {
                return null;
            }
            ViewModel.Email _email = new ViewModel.Email()
            {
                ID = email.ID,
                Body = email.Body,
                DateTimeCreated = email.DateTimeCreated,
                DateTimeReceived = email.DateTimeReceived,
                DateTimeSent = email.DateTimeSent,
                TicketID = email.TicketID,
                Subject = email.Subject,
                PreviousEmail = email.PreviousEmail,
                //From = email.From
            };
            //foreach (var attachmentId in email.AttachmentIds.Split(';'))
            //{
            //    if (!string.IsNullOrEmpty(attachmentId))
            //    {
            //        _email.AttachmentIds.Add();
            //    }
            //}
            return _email;
        }
    }
}
