﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.DBManagers
{
    public class UserManager
    {
        public bool UpsertUser(ViewModel.User user)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                Database.User userToBeUpdated = ConvertToDatabaseObject(user);
                if (user.ID == 0)
                {
                    context.Users.Add(userToBeUpdated);
                }
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
        public ViewModel.User ConverToViewModelObject(Database.User user)
        {
            if (user == null)
            {
                return null;
            }
            return new ViewModel.User()
            {
                ID = user.ID,
                Email = user.Email,
                EmployeeId = user.EmployeeId,
                IsActive = user.IsActive,
                Name = user.Name,
                IsExternalUser = user.IsExternalUser,
                Supervisor = user.Supervisor,
                PhoneNumber = user.PhoneNumber
            };
        }
        public ViewModel.User GetUserById(int id)
        {
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                return ConverToViewModelObject(context.Users.FirstOrDefault());
            }
        }
        public Database.User ConvertToDatabaseObject(ViewModel.User user)
        {
            if (user == null)
            {
                return null;
            }
            return new Database.User()
            {
                ID = user.ID,
                Name = user.Name,
                Email = user.Email,
                EmployeeId = user.EmployeeId,
                IsActive = user.IsActive,
                IsExternalUser = user.IsExternalUser,
                Supervisor = user.Supervisor,
                PhoneNumber = user.PhoneNumber
            };
        }

    }
}