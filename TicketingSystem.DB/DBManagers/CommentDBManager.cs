using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class CommentDBManager : ICommentManager
    {
        public bool UpsertComment(ViewModel.Comment comment)
        {
            HistoryDBManager historyDBManager = new HistoryDBManager();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {

                Database.Comment commentToBeUpdated = new Database.Comment();

                if (comment.ID != 0)
                {
                    commentToBeUpdated = context.Comments.FirstOrDefault(x => x.ID == comment.ID);
                }

                commentToBeUpdated.ID = comment.ID;
                commentToBeUpdated.Created = comment.Created;
                commentToBeUpdated.CreatedBy = comment.CreatedBy.ID;
                commentToBeUpdated.Details = comment.Details;
                commentToBeUpdated.IsPrivate = comment.IsPrivate;
                commentToBeUpdated.Modified = comment.Modified;
                commentToBeUpdated.ModifiedBy = comment.ModifiedBy.ID;
                commentToBeUpdated.Ticket = comment.TicketId;
                foreach (var attachemnt in comment.Attachments)
                {
                    commentToBeUpdated.AttachmentIds += attachemnt.ID + ",";
                }

                if (comment.ID == 0)
                {
                    commentToBeUpdated.Created = DateTime.Now;
                    context.Comments.Add(commentToBeUpdated);

                }
                bool isUpdated = false;
                isUpdated = Convert.ToBoolean(context.SaveChanges());
                comment.ID = commentToBeUpdated.ID;
                historyDBManager.CreateNewCommentHistory(comment);
                return isUpdated;
            }
        }
        public List<ViewModel.Comment> GetCommentsByTicketId(int ticketId)
        {
            List<ViewModel.Comment> comments = new List<ViewModel.Comment>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var comment in context.Comments.Where(x => x.Ticket == ticketId))
                {
                    comments.Add(ConvertToViewModelObject(comment));
                }
            }
            return comments;
        }
        public List<ViewModel.Comment> ConvertToViewModelObjects(List<Database.Comment> dBComments)
        {
            List<ViewModel.Comment> comments = new List<ViewModel.Comment>();
            foreach (var comment in dBComments)
            {
                comments.Add(ConvertToViewModelObject(comment));
            }
            return comments;
        }
        private ViewModel.Comment ConvertToViewModelObject(Database.Comment comment)
        {
            UserManager usermanager = new UserManager();
            AttachmentDBManager attachmentDBmanager = new AttachmentDBManager();
            TicketDBManager ticketDBManager = new TicketDBManager();
            var _comment = new ViewModel.Comment()
            {
                ID = comment.ID,
                IsPrivate = comment.IsPrivate,
                Details = comment.Details,
                TicketId = comment.Ticket,
                CreatedBy = usermanager.ConverToViewModelObject(comment._CreatedBy),
                ModifiedBy = usermanager.ConverToViewModelObject(comment._ModifiedBy),
                Created = comment.Created,
                Modified = comment.Modified,
            };
            var _attachment = attachmentDBmanager.GetAttachmentsDetailByTicketId(comment.Ticket);
            foreach (var attachmentId in comment.AttachmentIds.Split(','))
            {
                var attachment = _attachment.FirstOrDefault(x => Convert.ToString(x.ID) == attachmentId);
                if (attachment != null)
                {
                    _comment.Attachments.Add(attachment);
                }
            }
            return _comment;
        }
    }
}
