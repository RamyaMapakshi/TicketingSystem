using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IImpactManager
    {
        List<Impact> GetAllImpacts();
        bool UpsertImpact(Impact impact);
    }
}