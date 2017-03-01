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
        public bool UpsertCategory(ViewModel.Priority priority)
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
                    DaysDue = priority.DaysDue
                };
                if (priority.ID == 0)
                {
                    context.Priorities.Add(priorityToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
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
                DaysDue = priority.DaysDue
            };
        }
    }
}
