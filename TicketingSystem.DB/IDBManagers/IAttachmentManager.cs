using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IAttachmentManager
    {
        bool UpsertAttachment(ViewModel.Attachment attachment);
        List<ViewModel.Attachment> GetAttachmentsByTicketId(int ticketId);
    }
}
