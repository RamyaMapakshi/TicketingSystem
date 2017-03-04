using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IAttachmentManager
    {
        bool SaveAttachmentDetail(ViewModel.Attachment attachment);
        List<ViewModel.Attachment> GetAttachmentsDetailByTicketId(int ticketId);
    }
}
