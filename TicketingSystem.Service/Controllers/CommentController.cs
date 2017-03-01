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
    public class CommentController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public CommentController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Comment/SaveComment/")]
        public bool SaveComment([FromBody]Comment comment)
        {
            return dbManager.SaveComment(comment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [Route("api/Comment/GetCommentsByTicketId/{ticketId}")]
        public List<Comment> GetCommentsByTicketId(int ticketId)
        {
            return dbManager.GetCommentsByTicketId(ticketId);
        }
    }
}
