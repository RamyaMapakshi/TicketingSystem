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
            if (ticketType==null)
            {
                return null;
            }
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
                Database.TicketType ticketTypeToBeUpdated = new Database.TicketType();

                if (ticketType.ID != 0)
                {
                    ticketTypeToBeUpdated = context.TicketTypes.FirstOrDefault(x => x.ID == ticketType.ID);
                }

                ticketTypeToBeUpdated.ID = ticketType.ID;
                ticketTypeToBeUpdated.IsActive = ticketType.IsActive;
                ticketTypeToBeUpdated.Title = ticketType.Title;
                ticketTypeToBeUpdated.Description = ticketType.Description;
                ticketTypeToBeUpdated.IsDefault = ticketType.IsDefault;

                if ((bool)ticketType.IsActive && (bool)ticketType.IsDefault)
                {
                    var defaultTicketType = context.Status.FirstOrDefault(x => (bool)x.IsDefault && (bool)x.IsActive && x.ID != ticketType.ID);
                    if (defaultTicketType != null)
                    {
                        defaultTicketType.IsDefault = false;
                    }
                }

                if (ticketType.ID == 0)
                {
                    context.TicketTypes.Add(ticketTypeToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
    }
}
