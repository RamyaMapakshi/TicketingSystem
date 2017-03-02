using System.Collections.Generic;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.IDBManagers
{
    public interface ISubCategoryManager
    {
        List<SubCategory> GetAllsubCategories();
        bool UpsertSubCategory(SubCategory subCategory);
    }
}