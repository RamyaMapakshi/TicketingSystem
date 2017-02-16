using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB;

namespace TicketingSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DBManager db = new DBManager();
            
            var y=db.GetAllTickets();
        }
    }
}
