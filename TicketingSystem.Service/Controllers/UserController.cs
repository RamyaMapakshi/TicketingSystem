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
    public class UserController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public UserController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Route("api/GetUserById/{id}")]
        public User GetUserById(int id)
        {
            return dbManager.GetUserById(id);
        }
    }
}
