using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.Database;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class StatusDBManager : IStatusManager
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
        public string GetTicketStatusById(int id)
        {
            TicketDBManager ticketDBManager = new TicketDBManager();
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                return ticketDBManager.ConvertToViewModelObject(context.Tickets.FirstOrDefault(x => x.ID == id)).Status.Title;
            }
        }
        public List<ViewModel.Status> GetAllStatus()
        {
            List<ViewModel.Status> statusList = new List<ViewModel.Status>();
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                foreach (var status in context.Status)
                {
                    statusList.Add(CommonDBManager.StatusDBManager.ConvertToViewModelObject(status));
                }
            }
            return statusList;
        }
        public bool UpsertStatus(ViewModel.Status status)
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
