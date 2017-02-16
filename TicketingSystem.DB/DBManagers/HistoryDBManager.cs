using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.DBManagers
{
    public class HistoryDBManager
    {
        public bool UpsertHistory(ViewModel.History history)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.History historyToBeUpdated = new Database.History()
                {
                    ID = history.ID,
                    Action = history.Action,
                    ActionDateTime = history.ActionDateTime,
                    ActionTakenBy = history.ActionTakenBy.ID,
                    ActionType = history.ActionType.ToString(),
                    PreviousHistoryId = history.PreviousHistoryId,
                    TicketId = history.TicketId,
                    Status = history.Status
                };
                if (history.ID == 0)
                {
                    historyToBeUpdated.ActionDateTime = DateTime.Now;
                    context.Histories.Add(historyToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public List<ViewModel.History> GetHistoriesByTicketId(int ticketId)
        {
            List<ViewModel.History> histories = new List<ViewModel.History>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var history in context.Histories.Where(x => x.TicketId == ticketId))
                {
                    histories.Add(ConvertToViewModelObject(history));
                }
            }
            return histories;
        }
        public ViewModel.History GetLastHistoryByTicketId(int ticketId)
        {
            ViewModel.History history = new ViewModel.History();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                history = ConvertToViewModelObject(context.Histories.OrderByDescending(x => x.ID).FirstOrDefault());
            }
            return history;
        }
        public void ComapreChangesInTicket(ViewModel.Ticket ticket, ViewModel.Ticket oldTicket)
        {
            foreach (var history in GetChnagedValueHistory(ticket,oldTicket))
            {
                UpsertHistory(history);
            }
        }
        public List<ViewModel.History> GetChnagedValueHistory(ViewModel.Ticket newTicket, ViewModel.Ticket oldTicket)
        {
            List<ViewModel.History> newHistories = new List<ViewModel.History>();
            var previousHistory = GetLastHistoryByTicketId(newTicket.ID);

            foreach (var newItemProperty in newTicket.GetType().GetProperties())
            {
                var newValue = newItemProperty.GetValue(newTicket);
                var oldValue = oldTicket.GetType().GetProperties().FirstOrDefault(x => x.Name == newItemProperty.Name).GetValue(oldTicket);
                if (newValue != oldValue)
                {
                    newHistories.Add(new ViewModel.History()
                    {
                        Action = ChnagesInTicketField(newItemProperty.Name, Convert.ToString(oldValue), Convert.ToString(newValue)),
                        ActionDateTime = DateTime.Now,
                        ActionTakenBy = newTicket.ModifiedBy,
                        PreviousHistoryId = previousHistory.ID,
                        Status = newTicket.Status.Title,
                        TicketId = newTicket.ID,
                        ActionType = newItemProperty.Name
                    });
                }
            }
            return newHistories;
        }
        private string ChnagesInTicketField(string field, string oldValue, string newValue)
        {
            return field + " was changed from " + oldValue + " to " + newValue + ".";
        }
        public bool CreateTicketHistory(ViewModel.Ticket ticket)
        {
            return UpsertHistory(new ViewModel.History()
            {
                ID = 0,
                Action = "New ticket created" +((bool)ticket.IsTicketGeneratedViaEmail?" via Email":""),
                ActionDateTime = DateTime.Now,
                ActionTakenBy = ticket.CreatedBy,
                ActionType = "New Ticket",
                PreviousHistoryId = null,
                TicketId = ticket.ID
            });
        }
        public bool CreateNewCommentHistory(ViewModel.Comment comment)
        {
            return UpsertHistory(new ViewModel.History()
            {
                ID = 0,
                Action = "New comment added at " + DateTime.Now,
                ActionDateTime = DateTime.Now,
                ActionTakenBy = comment.CreatedBy,
                ActionType = "New Comment",
                PreviousHistoryId = GetLastHistoryByTicketId(comment.TicketId).ID,
                TicketId = comment.TicketId,
                Status = StatusDBManager.GetTicketStatusById(comment.TicketId)
            });
        }
        public bool CreateNewAttachmentHistory(ViewModel.Attachment attachment)
        {
            return UpsertHistory(new ViewModel.History()
            {
                ID = 0,
                Action = "New attachment uploaded at " + DateTime.Now,
                ActionDateTime = DateTime.Now,
                ActionTakenBy = attachment.UploadedBy,
                ActionType = "New Attachment",
                PreviousHistoryId = GetLastHistoryByTicketId(attachment.TicketId).ID,
                TicketId = attachment.TicketId,
                Status = StatusDBManager.GetTicketStatusById(attachment.TicketId)
            });
        }
        public List<ViewModel.History> ConvertToViewModelObjects(List<Database.History> dBComments)
        {
            List<ViewModel.History> histories = new List<ViewModel.History>();
            foreach (var history in dBComments)
            {
                histories.Add(ConvertToViewModelObject(history));
            }
            return histories;
        }
        private ViewModel.History ConvertToViewModelObject(Database.History history)
        {
            if (history == null)
            {
                return null;
            }
            UserManager usermanager = new UserManager();
            return new ViewModel.History()
            {
                ID = history.ID,
                Action = history.Action,
                TicketId = history.TicketId,
                ActionDateTime = history.ActionDateTime,
                PreviousHistoryId = history.PreviousHistoryId,
                ActionType = history.ActionType,
                ActionTakenBy = usermanager.ConverToViewModelObject(history._ActionTakenBy)
            };
        }
    }
}
