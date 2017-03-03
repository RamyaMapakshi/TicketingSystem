using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class PriorityDBManager : IPriorityManager
    {
        public bool UpsertPriority(ViewModel.Priority priority)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Priority priorityToBeUpdated = new Database.Priority()
                {
                    ID = priority.ID,
                    IsActive = priority.IsActive,
                    Title = priority.Title,
                    Description = priority.Description,
                    Color = priority.Color,
                    DaysDue = priority.DaysDue,
                    IsDefault = priority.IsDefault
                };
                if (priority.ID == 0)
                {
                    context.Priorities.Add(priorityToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public List<ViewModel.Priority> GetAllPriorities()
        {
            List<ViewModel.Priority> priorityList = new List<ViewModel.Priority>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var priority in context.Priorities)
                {
                    priorityList.Add(CommonDBManager.PriorityDBManager.ConvertToViewModelObject(priority));
                }
            }
            return priorityList;
        }
        public ViewModel.Priority ConvertToViewModelObject(Database.Priority priority)
        {
            return new ViewModel.Priority()
            {
                ID = priority.ID,
                IsActive = priority.IsActive,
                Title = priority.Title,
                Description = priority.Description,
                Color = priority.Color,
                DaysDue = priority.DaysDue,
                IsDefault = priority.IsDefault
            };
        }
    }
}
