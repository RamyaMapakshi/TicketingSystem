using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IEmailDBManager
    {
        bool SaveEmailDetailsInDB(ViewModel.Email email);

        bool SendEmail(ViewModel.Email email);
        int GetPreviousEmailByTicketId(int ticketId);
    }
}
