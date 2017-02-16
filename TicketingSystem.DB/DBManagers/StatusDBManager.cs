using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.Database;

namespace TicketingSystem.DB.DBManagers
{
    public class StatusDBManager
    {
        public ViewModel.Status ConvertToViewModelObject(Database.Status status)
        {
            return new ViewModel.Status()
            {
                Description = status.Description,
                ID = status.ID,
                Title = status.Title,
                IsActive = status.IsActive
            };
        }
        public static string GetTicketStatusById(int id)
        {
            TicketDBManager ticketDBManager = new TicketDBManager();
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                return ticketDBManager.ConvertToViewModelObject(context.Tickets.FirstOrDefault(x => x.ID == id)).Status.Title;
            }
        }
        public bool UpsertCategory(ViewModel.Status status)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Status statusToBeUpdated = new Database.Status()
                {
                    ID = status.ID,
                    IsActive = status.IsActive,
                    Title = status.Title,
                    Description = status.Description
                };
                if (status.ID == 0)
                {
                    context.Status.Add(statusToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
    }
}
