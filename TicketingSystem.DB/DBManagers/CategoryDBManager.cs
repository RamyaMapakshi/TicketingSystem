using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.DBManagers
{
    public class CategoryDBManager
    {
        public bool UpsertCategory(ViewModel.Category category)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.Category categoryToBeUpdated = new Database.Category()
                {
                    ID = category.ID,
                    IsActive = category.IsActive,
                    Title = category.Title,
                    Description = category.Description
                };
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
            return new ViewModel.Category()
            {
                Description = category.Description,
                ID = category.ID,
                Title = category.Title,
                IsActive = category.IsActive
            };
        }
    }
}
