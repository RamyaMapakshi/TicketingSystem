﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.DBManagers;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB
{
    public class DBManager
    {
        public List<Ticket> GetAllTickets()
        {
            return CommonDBManager.TickerDbManager.GetAllTickets();
        }
        public string UpsertTicketObject(Ticket ticket)
        {
            ticket = CommonDBManager.TickerDbManager.UpsertTicket(ticket);
            if (ticket.Attachments.Where(x => x.ID == 0).ToList().Count > 0)
            {
                foreach (var attachment in ticket.Attachments.Where(x => x.ID == 0))
                {
                    attachment.TicketId = ticket.ID;
                    UploadAttachment(attachment);
                }
            }
            if (ticket.Comments.Where(x=>x.ID==0).ToList().Count>0)
            {
                foreach (var comment in ticket.Comments)
                {
                    comment.TicketId = ticket.ID;
                    SaveComment(comment);
                }
            }
            return "";
        }
        public bool SaveComment(Comment comment)
        {
            if (comment.Attachment!=null)
            {
                UploadAttachment(comment.Attachment);
            }
            return CommonDBManager.CommentDbManager.UpsertComment(comment);
        }
        public bool UploadAttachment(Attachment attachment)
        {
            return CommonDBManager.AttachmentDBManager.UpsertAttachment(attachment);
        }
        public string UploadAttachments(List<Attachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                UploadAttachment(attachment);
            }
            return "";
        }
        public User GetUserById(int id)
        {
            return CommonDBManager.UserManager.GetUserById(1);
        }
        
        
    }
}
