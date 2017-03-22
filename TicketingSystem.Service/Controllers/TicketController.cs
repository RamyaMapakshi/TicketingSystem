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
        public int CreateTicket([FromBody]NewTicketCreationInfoBasic ticket)
        {
            return dbManager.UpsertTicketObject(ticket.CreateTicketFromBasicTicketInfo());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        
        [Route("api/Ticket/GetAllTicketTemplates/")]
        public List<TicketTemplate> GetAllTicketTemplates()
        {
            return dbManager.GetAllTicketTemplates();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Ticket/UpdateTicketInfo/Basic")]
        public int UpdateTicketInfo([FromBody]BasicViewTicket ticket)
        {
            return dbManager.UpsertTicketObject(ticket.ConvertBasicTicketToViewModelTicket());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Ticket/UpdateTicketInfo/")]
        public int UpdateTicketInfo([FromBody]Ticket ticket)
        {
            return dbManager.UpsertTicketObject(ticket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDependeciesToBeLoadedWithTicket"></param>
        /// <returns></returns>
        [Route("api/Ticket/GetTicketById/{id}/{isDependeciesToBeLoadedWithTicket}")]
        [Route("api/Ticket/GetTicketById/{id}")]
        public Ticket GetTicketById(int id,bool isDependeciesToBeLoadedWithTicket=false)
        {
            return dbManager.GetTicketById(id, isDependeciesToBeLoadedWithTicket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/Ticket/GetAllTickets/{isDependeciesToBeLoadedWithTicket}")]
        [Route("api/Ticket/GetAllTickets/")]
        public List<Ticket> GetAllTickets(bool isDependeciesToBeLoadedWithTicket=false)
        {
            return dbManager.GetAllTickets(isDependeciesToBeLoadedWithTicket);
        }
        
        
        
        
    }
}
