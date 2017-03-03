using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.DB;
using TicketingSystem.DB.ViewModel;
using TicketingSystem.Service.Models;

namespace TicketingSystem.Service.Controllers
{
    public class TicketController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public TicketController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Ticket/CreateTicketWithBasicInfo/")]
        public bool CreateTicket([FromBody]NewTicketCreationInfoBasic ticket)
        {
            return dbManager.UpsertTicketObject(ticket.CreateTicketFromBasicTicketInfo());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Ticket/GetTicketById/")]
        public Ticket GetTicketById(int id)
        {
            return dbManager.GetTicketById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/Ticket/GetAllTickets/")]
        public List<Ticket> GetAllTickets()
        {
            return dbManager.GetAllTickets();
        }
        
        
        
        
    }
}
