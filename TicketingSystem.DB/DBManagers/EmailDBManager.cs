using System;
using System.Collections.Generic;
using System.Linq;
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
                var _email= context.Emails.Where(x => x.TicketID == ticketId).OrderByDescending(x => x.ID).FirstOrDefault();
                if (_email==null)
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
                emailObjToBeUpdated.From = email.From;
                emailObjToBeUpdated.TicketID = email.TicketID;
                emailObjToBeUpdated.Subject = email.Subject;
                emailObjToBeUpdated.PreviousEmail = email.PreviousEmail;
                emailObjToBeUpdated.ID = email.ID;
                foreach (var attachmentId in email.Attachments)
                {
                    emailObjToBeUpdated.AttachmentIds += attachmentId + ",";
                }
                foreach (var to in email.To)
                {
                    emailObjToBeUpdated.To += to + ",";
                }
                foreach (var cc in email.CC)
                {
                    emailObjToBeUpdated.CC += cc + ",";
                }
                context.Emails.Add(emailObjToBeUpdated);
                context.SaveChanges();
                return true;
            }
        }

        private ViewModel.Email ConvertToViewModelObject(Database.Email email)
        {
            if (email == null)
            {
                return null;
            }
            ViewModel.Email _email= new ViewModel.Email()
            {
                ID = email.ID,
                Body = email.Body,
                DateTimeCreated = email.DateTimeCreated,
                DateTimeReceived = email.DateTimeReceived,
                DateTimeSent = email.DateTimeSent,
                TicketID = email.TicketID,
                Subject = email.Subject,
                PreviousEmail = email.PreviousEmail,
                From = email.From
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
