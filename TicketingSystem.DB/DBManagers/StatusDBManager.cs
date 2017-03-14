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
            if (status==null)
            {
                return null;
            }
            return new ViewModel.Status()
            {
                Description = status.Description,
                ID = status.ID,
                Title = status.Title,
                IsActive = status.IsActive,
                IsDefault = status.IsDefault
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
                Database.Status statusToBeUpdated = new Database.Status();
                if (status.ID != 0)
                {
                    statusToBeUpdated = context.Status.FirstOrDefault(x => x.ID == status.ID);
                }
                statusToBeUpdated.ID = status.ID;
                statusToBeUpdated.IsActive = status.IsActive;
                statusToBeUpdated.Title = status.Title;
                statusToBeUpdated.Description = status.Description;
                statusToBeUpdated.IsDefault = status.IsDefault;
                if ((bool)status.IsActive&&(bool)status.IsDefault)
                {
                    var defaultStatus = context.Status.FirstOrDefault(x=>(bool)x.IsDefault&&(bool)x.IsActive&&x.ID!=status.ID);
                    if (defaultStatus!=null)
                    {
                        defaultStatus.IsDefault = false;
                    }
                }
                if (status.ID == 0)
                {
                    context.Status.Add(statusToBeUpdated);
                }

                return Convert.ToBoolean(context.SaveChanges());
            }
        }
    }
}
