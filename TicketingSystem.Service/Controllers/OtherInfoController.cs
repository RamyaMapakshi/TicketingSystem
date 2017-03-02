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
    public class OtherInfoController : ApiController
    {

        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public OtherInfoController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/OtherInfo/Status/GetAllStatus/")]
        public List<Status> GetAllStatus()
        {
            return dbManager.GetAllStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/OtherInfo/TicketType/GetAllTicketTypes/")]
        public List<TicketType> GetAllTicketTypes()
        {
            return dbManager.GetAllTicketTypes();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/OtherInfo/Priority/GetAllPriorities/")]
        public List<Priority> GetAllPriorities()
        {
            return dbManager.GetAllPriorities();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/OtherInfo/Category/GetAllCategories/")]
        public List<Category> GetAllCategories()
        {
            return dbManager.GetAllCategories();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/OtherInfo/SubCategory/GetAllSubCategories/")]
        public List<SubCategory> GetAllSubCategories()
        {
            return dbManager.GetAllSubCategory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/OtherInfo/Impact/GetAllImpacts/")]
        public List<Impact> GetAllImpacts()
        {
            return dbManager.GetAllImpacts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/OtherInfo/Impact/UpsertImpact/")]
        public bool UpsertImpact([FromBody]Impact impact)
        {
            return dbManager.UpsertImpact(impact);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SubCategory"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/OtherInfo/SubCategory/UpsertSubCategory/")]
        public bool UpsertSubCategory([FromBody]SubCategory subCategory)
        {
            return dbManager.UpsertSubCategory(subCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/OtherInfo/Status/UpsertStatus/")]
        public bool UpsertStatus([FromBody]Status status)
        {
            return dbManager.UpsertStatus(status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/OtherInfo/Priority/UpsertPriority/")]
        public bool UpsertPriority([FromBody]Priority priority)
        {
            return dbManager.UpsertPriority(priority);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/OtherInfo/TicketType/UpsertTicketType/")]
        public bool UpsertTicketType([FromBody]TicketType type)
        {
            return dbManager.UpsertTicketType(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/OtherInfo/Category/UpsertCategory/")]
        public bool UpsertCategory([FromBody]Category category)
        {
            return dbManager.UpsertCategory(category);
        }
    }
}
