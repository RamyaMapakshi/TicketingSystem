using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.DB;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.Service.Controllers
{
    public class TicketController : ApiController
    {
        IDBManager dbManager;
        public TicketController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }
        [Route("api/GetAllTickets/")]
        public List<Ticket> GetAllTickets()
        {
            return dbManager.GetAllTickets();
        }
        [Route("api/GetUserById/{id}")]
        public User GetUserById(int id)
        {
            return dbManager.GetUserById(id);
        }
        [HttpPost]
        [Route("api/UpsertStatus/{status}")]
        public bool UpsertStatus([FromBody]Status status)
        {
            return dbManager.UpsertStatus(status);
        }
    }
}
