﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.DBManagers
{
    public class AttachmentDBManager
    {
        public bool UpsertAttachment(ViewModel.Attachment attachment)
        {
            HistoryDBManager historyDBManager = new HistoryDBManager();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Attachment attachmentToBeUpdated = new Database.Attachment()
                {
                    ID = attachment.ID,
                    FileName = attachment.FileName,
                    FileUrl = attachment.FileUrl,
                    Ticket = attachment.TicketId,
                    UploadedBy = attachment.UploadedBy.ID
                };
                if (attachment.ID==0)
                {
                    context.Attachments.Add(attachmentToBeUpdated);
                    historyDBManager.CreateNewAttachmentHistory(attachment);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public List<ViewModel.Attachment> GetAttachmentsByTicketId(int ticketId)
        {
            List<ViewModel.Attachment> attachments = new List<ViewModel.Attachment>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var attachment in context.Attachments.Where(x => x.Ticket == ticketId))
                {
                    attachments.Add(ConvertToViewModelObject(attachment));
                }
            }
            return attachments;
        }
        public List<ViewModel.Attachment> ConvertToViewModelObjects(List<Database.Attachment> DBAttachments)
        {
            List<ViewModel.Attachment> attachments = new List<ViewModel.Attachment>();
            foreach (var attachment in DBAttachments)
            {
                attachments.Add(ConvertToViewModelObject(attachment));
            }
            return attachments;
        }
        public ViewModel.Attachment ConvertToViewModelObject(Database.Attachment attachment)
        {
            if (attachment==null)
            {
                return null;
            }
            UserManager usermanager = new UserManager();
            return new ViewModel.Attachment()
            {
                ID = attachment.ID,
                FileName = attachment.FileName,
                FileUrl = attachment.FileUrl,
                TicketId = attachment.Ticket,
                UploadedBy = usermanager.ConverToViewModelObject(attachment._UploadedBy)
            };
        }
    }
}