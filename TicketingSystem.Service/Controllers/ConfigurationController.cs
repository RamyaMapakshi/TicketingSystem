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
    public class ConfigurationController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public ConfigurationController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns>g</returns>
        [HttpPost]
        [Route("api/Configuration/UpdateConfiguration/")]
        public bool UpdateConfiguration([FromBody]Configuration config)
        {
            return dbManager.UpdateConfiguration(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/Configuration/GetAllConfigurations/")]
        public List<Configuration> GetAllConfigurations()
        {
            return dbManager.GetAllConfigurations();
        }
    }
}
