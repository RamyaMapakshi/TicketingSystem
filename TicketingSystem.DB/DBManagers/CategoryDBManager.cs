using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class CategoryDBManager : ICategoryManager
    {
        public bool UpsertCategory(ViewModel.Category category)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Category categoryToBeUpdated = new Database.Category();

                if (category.ID != 0)
                {
                    categoryToBeUpdated = context.Categories.FirstOrDefault(x => x.ID == categoryToBeUpdated.ID);
                }
                categoryToBeUpdated.ID = category.ID;
                categoryToBeUpdated.IsActive = category.IsActive;
                categoryToBeUpdated.Title = category.Title;
                categoryToBeUpdated.Description = category.Description;
                categoryToBeUpdated.IsDefault = category.IsDefault;

                if ((bool)category.IsActive && (bool)category.IsDefault)
                {
                    var defaultCategory = context.Status.FirstOrDefault(x => (bool)x.IsDefault && (bool)x.IsActive && x.ID != category.ID);
                    if (defaultCategory != null)
                    {
                        defaultCategory.IsDefault = false;
                    }
                }

                if (category.ID == 0)
                {
                    context.Categories.Add(categoryToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public List<ViewModel.Category> GetAllCategories()
        {
            List<ViewModel.Category> categories = new List<ViewModel.Category>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var category in context.Categories)
                {
                    categories.Add(ConvertToViewModelObject(category));
                }
            }
            return categories;
        }

        public ViewModel.Category ConvertToViewModelObject(Database.Category category)
        {
            if (category==null)
            {
                return null;
            }
            return new ViewModel.Category()
            {
                Description = category.Description,
                ID = category.ID,
                Title = category.Title,
                IsActive = category.IsActive,
                IsDefault = category.IsDefault
            };
        }
    }
}
