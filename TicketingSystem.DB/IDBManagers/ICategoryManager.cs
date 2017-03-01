using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ICategoryManager
    {
        bool UpsertCategory(ViewModel.Category category);
        List<ViewModel.Category> GetAllCategories();
    }
}
