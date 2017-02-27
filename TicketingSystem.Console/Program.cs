using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB;
using TicketingSystem.DB.Classes.Models;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DBManager db = new DBManager();

            var y = db.GetAllTickets();
            NewTicketCreationInfoBasic t = new NewTicketCreationInfoBasic();
            User user = new User()
            {
                ID = 1,
                Email = "vainktesh.kumar@kci.com",
                EmployeeId = "1423",
                IsActive = true,
                IsExternalUser = false,
                Name = "Vainktesh",
                PhoneNumber = "7416672038",
                Supervisor = ""
            };
            t.Attachments.Add(new DB.ViewModel.Attachment()
            {
                ID = 0,
                FileName = "a",
                FileUrl = "aa",
                UploadedBy=user
            });
            t.RequestedBy = user;
            t.RequestedFor = user;
            
            db.UpsertTicketObject(t.CreateTicketFromBasicTicketInfo());
        }
    }
}
