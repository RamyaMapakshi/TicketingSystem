using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class ImpactDBManager : IImpactManager
    {
        public bool UpsertImpact(ViewModel.Impact impact)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Impact impactToBeUpdated = new Database.Impact()
                {
                    ID = impact.ID,
                    IsActive = impact.IsActive,
                    Title = impact.Title,
                    Description = impact.Description,
                    IsDefault = impact.IsDefault
                };
                if (impact.ID == 0)
                {
                    context.Impacts.Add(impactToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public List<ViewModel.Impact> GetAllImpacts()
        {
            List<ViewModel.Impact> impacts = new List<ViewModel.Impact>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var impact in context.Impacts)
                {
                    impacts.Add(ConvertToViewModelObject(impact));
                }
            }
            return impacts;
        }

        public ViewModel.Impact ConvertToViewModelObject(Database.Impact impact)
        {
            return new ViewModel.Impact()
            {
                Description = impact.Description,
                ID = impact.ID,
                Title = impact.Title,
                IsActive = impact.IsActive,
                IsDefault = impact.IsDefault
            };
        }
    }
}
