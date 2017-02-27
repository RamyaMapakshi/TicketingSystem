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
            foreach (var history in GetChnagedValueHistory(ticket, oldTicket))
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
                if (newItemProperty.Name.Equals("Created")|| newItemProperty.Name.Equals("CreatedBy")|| newItemProperty.Name.Equals("Modified") || newItemProperty.Name.Equals("ModifiedBy"))
                {
                    continue;
                }
                var newValue = newItemProperty.GetValue(newTicket);
                var oldValue = oldTicket.GetType().GetProperties().FirstOrDefault(x => x.Name == newItemProperty.Name).GetValue(oldTicket);
                bool isValuesEqual = true;
                if (newValue == null && oldValue != null)
                {
                    newValue = "Empty";
                    if (oldValue.GetType() == typeof(string))
                    {
                        isValuesEqual = newValue.Equals(oldValue);
                    }
                    else if (oldValue.GetType() == typeof(Int32))
                    {
                        oldValue = Convert.ToString(oldValue);
                        isValuesEqual = newValue.Equals(oldValue);
                    }
                    else if (oldValue.GetType() == typeof(ViewModel.User))
                    {
                        ViewModel.User oldUserValue = (ViewModel.User)oldValue;
                        oldValue = oldUserValue.Name;
                    }
                    else if (oldValue.GetType() == typeof(ViewModel.Priority))
                    {
                        ViewModel.Priority oldUserValue = (ViewModel.Priority)oldValue;
                        oldValue = oldUserValue.Title;
                    }
                    else if (oldValue.GetType() == typeof(ViewModel.Status))
                    {
                        ViewModel.Status oldUserValue = (ViewModel.Status)oldValue;
                        oldValue = oldUserValue.Title;
                    }
                    else if (oldValue.GetType() == typeof(ViewModel.TicketType))
                    {
                        ViewModel.TicketType oldUserValue = (ViewModel.TicketType)oldValue;
                        oldValue = oldUserValue.Title;
                    }
                    else if (oldValue.GetType() == typeof(ViewModel.Category))
                    {
                        ViewModel.Category oldUserValue = (ViewModel.Category)oldValue;
                        oldValue = oldUserValue.Title;
                    }
                    oldValue = string.IsNullOrEmpty(Convert.ToString(oldValue)) ? "Empty" : Convert.ToString(oldValue);
                    isValuesEqual = newValue.Equals(oldValue);
                }
                else if (newValue != null && oldValue == null)
                {
                    oldValue = "Empty";
                    if (newValue.GetType() == typeof(string))
                    {
                        isValuesEqual = newValue.Equals(oldValue);
                    }
                    else if (newValue.GetType() == typeof(Int32))
                    {
                        newValue = Convert.ToString(newValue);
                        isValuesEqual = newValue.Equals(oldValue);
                    }
                    else if (newValue.GetType() == typeof(ViewModel.User))
                    {
                        ViewModel.User newUserValue = (ViewModel.User)newValue;
                        newValue = newUserValue.Name;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.Priority))
                    {
                        ViewModel.Priority newUserValue = (ViewModel.Priority)newValue;
                        newValue = newUserValue.Title;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.Status))
                    {
                        ViewModel.Status newUserValue = (ViewModel.Status)newValue;
                        newValue = newUserValue.Title;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.TicketType))
                    {
                        ViewModel.TicketType newUserValue = (ViewModel.TicketType)newValue;
                        newValue = newUserValue.Title;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.Category))
                    {
                        ViewModel.Category newUserValue = (ViewModel.Category)newValue;
                        newValue = newUserValue.Title;
                    }
                    oldValue = string.IsNullOrEmpty(Convert.ToString(oldValue)) ? "Empty" : Convert.ToString(oldValue);
                    isValuesEqual = newValue.Equals(oldValue);
                }
                else if (newValue != null && oldValue != null)
                {
                    if (newValue.GetType() == typeof(ViewModel.User))
                    {
                        ViewModel.User newUserValue = (ViewModel.User)newValue;
                        ViewModel.User oldUserValue = (ViewModel.User)oldValue;
                        newValue = newUserValue.Name;
                        oldValue = oldUserValue.Name;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.Priority))
                    {
                        ViewModel.Priority newUserValue = (ViewModel.Priority)newValue;
                        ViewModel.Priority oldUserValue = (ViewModel.Priority)oldValue;
                        newValue = newUserValue.Title;
                        oldValue = oldUserValue.Title;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.Status))
                    {
                        ViewModel.Status newUserValue = (ViewModel.Status)newValue;
                        ViewModel.Status oldUserValue = (ViewModel.Status)oldValue;
                        newValue = newUserValue.Title;
                        oldValue = oldUserValue.Title;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.TicketType))
                    {
                        ViewModel.TicketType newUserValue = (ViewModel.TicketType)newValue;
                        ViewModel.TicketType oldUserValue = (ViewModel.TicketType)oldValue;
                        newValue = newUserValue.Title;
                        oldValue = oldUserValue.Title;
                    }
                    else if (newValue.GetType() == typeof(ViewModel.Category))
                    {
                        ViewModel.Category newUserValue = (ViewModel.Category)newValue;
                        ViewModel.Category oldUserValue = (ViewModel.Category)oldValue;
                        newValue = newUserValue.Title;
                        oldValue = oldUserValue.Title;
                    }
                    newValue = string.IsNullOrEmpty(Convert.ToString(newValue)) ? "Empty" : Convert.ToString(newValue);
                    oldValue = string.IsNullOrEmpty(Convert.ToString(oldValue)) ? "Empty" : Convert.ToString(oldValue);
                    isValuesEqual = newValue.Equals(oldValue);
                }
                if (!isValuesEqual)
                {
                    newHistories.Add(new ViewModel.History()
                    {
                        Action = ChnagesInTicketField(newItemProperty.Name, Convert.ToString(oldValue), Convert.ToString(newValue)),
                        ActionDateTime = DateTime.Now,
                        ActionTakenBy = newTicket.ModifiedBy,
                        PreviousHistoryId = previousHistory.ID,
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
                Action = "New ticket created" + ((bool)ticket.IsTicketGeneratedViaEmail ? " via Email" : ""),
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
