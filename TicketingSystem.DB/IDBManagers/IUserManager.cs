using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IUserManager
    {
        ViewModel.User UpsertUser(ViewModel.User user);
        ViewModel.User GetUserByEmail(string email);
        ViewModel.User GetUserById(int id);
    }
}
