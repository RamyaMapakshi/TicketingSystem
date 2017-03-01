using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.DB;

namespace TicketingSystem.Service.Controllers
{
    public class UtilityController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public UtilityController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

    }
}
