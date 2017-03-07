using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.DB.ViewModel;
using TicketingSystem.Service.Models;

namespace TicketingSystem.Service.Classes
{
    public class Utility
    {
        public Ticket ConvertBasicTicketToViewModelTicket(BasicViewTicket bTicket)
        {
            Ticket ticket = new Ticket();
            foreach (var property in bTicket.GetType().GetProperties())
            {
                ticket.GetType().GetProperties().FirstOrDefault(x => x.Name == property.Name).SetValue(bTicket,property.GetValue(bTicket));
            }
            ticket.Modified = DateTime.Now;
            return ticket;
        }
    }
}