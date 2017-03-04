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
    public class AttachmentController : ApiController
    {
        IDBManager dbManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbManager"></param>
        public AttachmentController(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Attachment/SaveAttachmentDetail/")]
        public bool UploadAttachment([FromBody]Attachment attachment)
        {
            return dbManager.SaveAttachmentDetail(attachment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Attachment/SaveAttachmentsDetail/")]
        public bool UploadAttachments([FromBody]List<Attachment> attachments)
        {
            return dbManager.SaveAttachmentsDetail(attachments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [Route("api/Attachment/GetAttachmentsDetailByTicketId/{ticketId}")]
        public List<Attachment> GetAttachmentsByTicketId(int ticketId)
        {
            return dbManager.GetAttachmentsDetailByTicketId(ticketId);
        }
    }
}
