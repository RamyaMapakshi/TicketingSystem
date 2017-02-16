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
            foreach (var item in y)
            {
                item.DueDate = DateTime.Now;
                db.UpsertTicketObject(item);
            }
            var user = db.GetUserById(1);
            //db.SaveComment(new DB.ViewModel.Comment()
            //{
            //    ID = 0,
            //    Created = DateTime.Now,
            //    CreatedBy = user,
            //    Details = "sdsfsfdsd",
            //    IsPrivate = false,
            //    Modified = DateTime.Now,
            //    ModifiedBy = user,
            //    TicketId = new DB.ViewModel.Ticket(),
            //    Attachment=null
            //});

        }
    }
}
