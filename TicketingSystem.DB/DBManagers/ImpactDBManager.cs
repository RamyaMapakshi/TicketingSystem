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
                Database.Impact impactToBeUpdated = new Database.Impact();

                if (impact.ID!=0)
                {
                    impactToBeUpdated = context.Impacts.FirstOrDefault(x=>x.ID==impact.ID);
                }

                impactToBeUpdated.ID = impact.ID;
                impactToBeUpdated.IsActive = impact.IsActive;
                impactToBeUpdated.Title = impact.Title;
                impactToBeUpdated.Description = impact.Description;
                impactToBeUpdated.IsDefault = impact.IsDefault;

                if ((bool)impact.IsActive && (bool)impact.IsDefault)
                {
                    var defaultImpact = context.Impacts.FirstOrDefault(x => (bool)x.IsDefault && (bool)x.IsActive && x.ID != impact.ID);
                    if (defaultImpact != null)
                    {
                        defaultImpact.IsDefault = false;
                    }
                }

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
            if (impact==null)
            {
                return null;
            }
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
