using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class TicketTypeDBManager : ITicketTypeManager
    {
        public ViewModel.TicketType ConvertToViewModelObject(Database.TicketType ticketType)
        {
            return new ViewModel.TicketType()
            {
                Description = ticketType.Description,
                ID = ticketType.ID,
                Title = ticketType.Title,
                IsActive = ticketType.IsActive
            };
        }
        public bool UpsertCategory(ViewModel.TicketType ticketType)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.TicketType ticketTypeToBeUpdated = new Database.TicketType()
                {
                    ID = ticketType.ID,
                    IsActive = ticketType.IsActive,
                    Title = ticketType.Title,
                    Description = ticketType.Description
                };
                if (ticketType.ID == 0)
                {
                    context.TicketTypes.Add(ticketTypeToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
    }
}
