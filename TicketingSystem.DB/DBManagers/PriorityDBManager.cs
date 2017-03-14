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
                Database.Priority priorityToBeUpdated = new Database.Priority();

                if (priority.ID!=0)
                {
                    priorityToBeUpdated = context.Priorities.FirstOrDefault(x=>x.ID==priority.ID);
                }

                priorityToBeUpdated.ID = priority.ID;
                priorityToBeUpdated.IsActive = priority.IsActive;
                priorityToBeUpdated.Title = priority.Title;
                priorityToBeUpdated.Description = priority.Description;
                priorityToBeUpdated.Color = priority.Color;
                priorityToBeUpdated.DaysDue = priority.DaysDue;
                priorityToBeUpdated.IsDefault = priority.IsDefault;

                if ((bool)priority.IsActive && (bool)priority.IsDefault)
                {
                    var defaultPriority = context.Priorities.FirstOrDefault(x => (bool)x.IsDefault && (bool)x.IsActive && x.ID != priority.ID);
                    if (defaultPriority != null)
                    {
                        defaultPriority.IsDefault = false;
                    }
                }

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
            if (priority==null)
            {
                return null;
            }
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
