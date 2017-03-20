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
    public class EmailController : ApiController
    {

        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public EmailController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        [HttpPost]
        [Route("api/Email/CreateTicketViaEmail/")]
        public Email CreateTicketViaEmail([FromBody]Email email)
        {
            return dbManager.UpsertTicketViaEmail(email);
        }
    }
}
