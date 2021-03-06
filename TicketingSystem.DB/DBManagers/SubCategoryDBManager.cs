﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class SubCategoryDBManager : ISubCategoryManager
    {
        public bool UpsertSubCategory(ViewModel.SubCategory subCategory)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.SubCategory subCategoryToBeUpdated = new Database.SubCategory();

                if (subCategory.ID != 0)
                {
                    subCategoryToBeUpdated = context.SubCategories.FirstOrDefault(x => x.ID == subCategory.ID);
                }
                subCategoryToBeUpdated.ID = subCategory.ID;
                subCategoryToBeUpdated.IsActive = subCategory.IsActive;
                subCategoryToBeUpdated.Title = subCategory.Title;
                subCategoryToBeUpdated.Description = subCategory.Description;
                subCategoryToBeUpdated.IsDefault = subCategory.IsDefault;
                subCategoryToBeUpdated.ParentCategory = subCategory.ParentCategory;

                if (subCategory.ID == 0)
                {
                    context.SubCategories.Add(subCategoryToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public List<ViewModel.SubCategory> GetAllsubCategories()
        {
            List<ViewModel.SubCategory> categories = new List<ViewModel.SubCategory>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var subCategory in context.SubCategories)
                {
                    categories.Add(ConvertToViewModelObject(subCategory));
                }
            }
            return categories;
        }

        public ViewModel.SubCategory ConvertToViewModelObject(Database.SubCategory subCategory)
        {
            if (subCategory==null)
            {
                return null;
            }
            return new ViewModel.SubCategory()
            {
                Description = subCategory.Description,
                ID = subCategory.ID,
                Title = subCategory.Title,
                IsActive = subCategory.IsActive,
                ParentCategory = subCategory.ParentCategory,
                IsDefault = subCategory.IsDefault
            };
        }
    }
}
