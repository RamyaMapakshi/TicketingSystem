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
                IsActive = ticketType.IsActive,
                IsDefault = ticketType.IsDefault,
            };
        }
        public List<ViewModel.TicketType> GetAllTicketTypes()
        {
            List<ViewModel.TicketType> ticketTypeList = new List<ViewModel.TicketType>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var types in context.TicketTypes)
                {
                    ticketTypeList.Add(CommonDBManager.TicketTypeDBManager.ConvertToViewModelObject(types));
                }
            }
            return ticketTypeList;
        }
        public bool UpsertTicketType(ViewModel.TicketType ticketType)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.TicketType ticketTypeToBeUpdated = new Database.TicketType()
                {
                    ID = ticketType.ID,
                    IsActive = ticketType.IsActive,
                    Title = ticketType.Title,
                    Description = ticketType.Description,
                    IsDefault = ticketType.IsDefault
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
