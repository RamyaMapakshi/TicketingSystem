using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.Database;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class TicketDBManager : ITicketManager
    {
        public ViewModel.Ticket UpsertTicket(ViewModel.Ticket ticket)
        {
            HistoryDBManager historyDBManager = new HistoryDBManager();
            Database.Ticket ticketToBeUpdated = new Ticket()
            {
                AssignedTechnician = _retrunUserId(ticket.AssignedTechnician),
                ID = ticket.ID,
                ClosedBy = _retrunUserId(ticket.ClosedBy),
                ClosedDate = ticket.ClosedDate,
                Created = ticket.Created,
                CreatedBy = (int)_retrunUserId(ticket.CreatedBy),
                Description = ticket.Description,
                Tags = ticket.Tags,
                DueDate = ticket.DueDate,
                DuplicateTicketID = ticket.DuplicateTicketID,
                ExpectedCompletionDate = ticket.ExpectedCompletionDate,
                IsActive = ticket.IsActive,
                IsDuplicate = ticket.IsDuplicate,
                IsEscalated = ticket.IsEscalated,
                IsTicketGeneratedViaEmail = ticket.IsTicketGeneratedViaEmail,
                Modified = ticket.Modified,
                ModifiedBy = (int)_retrunUserId(ticket.ModifiedBy),
                TicketPriority = ticket.Priority.ID,
                TicketCategory = ticket.Category.ID,
                TicketStatus = ticket.Status.ID,
                TicketType = ticket.Type.ID,
                RequestedBy = (int)_retrunUserId(ticket.RequestedBy),
                ResolvedBy = _retrunUserId(ticket.ResolvedBy),
                ResolvedDate = ticket.ResolvedDate,
            };
            using (Database.TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                if (ticket.ID == 0)
                {
                    ticketToBeUpdated.Created = DateTime.Now;
                    context.Tickets.Add(ticketToBeUpdated);
                }
                else
                {
                    historyDBManager.ComapreChangesInTicket(ticket, GetTicketByID(ticket.ID));
                }

                context.SaveChanges();
                if (ticket.ID == 0)
                {
                    ticket.ID = ticketToBeUpdated.ID;
                    historyDBManager.CreateTicketHistory(ticket);
                }
                return ticket;
            }
        }

        private int? _retrunUserId(ViewModel.User user)
        {
            if (user == null)
            {
                return null;
            }
            return user.ID;
        }
        public List<ViewModel.Ticket> GetAllTickets()
        {
            List<ViewModel.Ticket> tickets = new List<ViewModel.Ticket>();
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                foreach (var ticket in context.Tickets)
                {
                    tickets.Add(ConvertToViewModelObject(ticket));
                }
            }
            return tickets;
        }
        public ViewModel.Ticket GetTicketByID(int id)
        {
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                return ConvertToViewModelObject(context.Tickets.FirstOrDefault(x => x.ID == id));
            }
        }
       
        public ViewModel.Ticket ConvertToViewModelObject(Database.Ticket ticket)
        {
            return new ViewModel.Ticket()
            {
                ID = ticket.ID,
                DueDate = ticket.DueDate,
                ClosedDate = ticket.ClosedDate,
                Created = ticket.Created,
                Modified = ticket.Modified,
                ResolvedDate = ticket.ResolvedDate,
                ExpectedCompletionDate = ticket.ExpectedCompletionDate,
                Description = ticket.Description,
                Tags = ticket.Tags,
                IsActive = ticket.IsActive,
                DuplicateTicketID = ticket.DuplicateTicketID,
                IsDuplicate = ticket.IsDuplicate,
                IsEscalated = ticket.IsEscalated,
                IsTicketGeneratedViaEmail = ticket.IsTicketGeneratedViaEmail,
                AssignedTechnician = CommonDBManager.UserManager.ConverToViewModelObject(ticket._AssignedTechnician),
                ClosedBy = CommonDBManager.UserManager.ConverToViewModelObject(ticket._ClosedBy),
                ModifiedBy = CommonDBManager.UserManager.ConverToViewModelObject(ticket._ModifiedBy),
                RequestedBy = CommonDBManager.UserManager.ConverToViewModelObject(ticket._RequestedBy),
                RequestedFor = CommonDBManager.UserManager.ConverToViewModelObject(ticket._RequestedFor),
                ResolvedBy = CommonDBManager.UserManager.ConverToViewModelObject(ticket._ResolvedBy),
                CreatedBy = CommonDBManager.UserManager.ConverToViewModelObject(ticket._CreatedBy),
                Category = CommonDBManager.CategoryDBManager.ConvertToViewModelObject(ticket.Category),
                Priority = CommonDBManager.PriorityDBManager.ConvertToViewModelObject(ticket.Priority),
                Status = CommonDBManager.StatusDBManager.ConvertToViewModelObject(ticket.Status),
                Type = CommonDBManager.TicketTypeDBManager.ConvertToViewModelObject(ticket.TicketType1),
                Attachments = CommonDBManager.AttachmentDBManager.ConvertToViewModelObjects(ticket.Attachments.ToList()),
                Comments = CommonDBManager.CommentDbManager.ConvertToViewModelObjects(ticket.Comments.ToList()),
                Histories = CommonDBManager.HistoryDBManager.ConvertToViewModelObjects(ticket.Histories.ToList())
            };
        }
    }
}
