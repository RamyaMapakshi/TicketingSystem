using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.DBManagers
{
    public class CommentDBManager
    {
        public bool UpsertComment(ViewModel.Comment comment)
        {
            HistoryDBManager historyDBManager = new HistoryDBManager();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Comment commentToBeUpdated = new Database.Comment()
                {
                    ID = comment.ID,
                    Created = comment.Created,
                    CreatedBy = comment.CreatedBy.ID,
                    Details = comment.Details,
                    IsPrivate = comment.IsPrivate,
                    Modified = comment.Modified,
                    ModifiedBy = comment.ModifiedBy.ID,
                    Ticket = comment.TicketId,
                    AttachmentId = comment.Attachment == null ? (int?)null : comment.Attachment.ID
                };
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
            return new ViewModel.Comment()
            {
                ID = comment.ID,
                IsPrivate = comment.IsPrivate,
                Details = comment.Details,
                TicketId = comment.Ticket,
                CreatedBy = usermanager.ConverToViewModelObject(comment._CreatedBy),
                ModifiedBy = usermanager.ConverToViewModelObject(comment._ModifiedBy),
                Created = comment.Created,
                Modified = comment.Modified,
                Attachment = attachmentDBmanager.ConvertToViewModelObject(comment._Ticket.Attachments.FirstOrDefault(x => x.ID == comment.AttachmentId))
            };
        }
    }
}
