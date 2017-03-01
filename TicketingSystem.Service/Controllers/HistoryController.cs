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
    public class HistoryController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public HistoryController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [Route("api/History/GetHistoriesByTicketId/{ticketId}")]
        public List<History> GetHistoriesByTicketId(int ticketId)
        {
            return dbManager.GetHistoriesByTicketId(ticketId);
        }
    }
}

