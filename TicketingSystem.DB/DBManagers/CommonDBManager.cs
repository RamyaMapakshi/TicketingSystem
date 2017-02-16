using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.DBManagers
{
    public class CommonDBManager
    {
        public static UserManager UserManager = new UserManager();
        public static AttachmentDBManager AttachmentDBManager = new AttachmentDBManager();
        public static CommentDBManager CommentDbManager = new CommentDBManager();
        public static CategoryDBManager CategoryDBManager = new CategoryDBManager();
        public static TicketTypeDBManager TicketTypeDBManager = new TicketTypeDBManager();
        public static StatusDBManager StatusDBManager = new StatusDBManager();
        public static PriorityDBManager PriorityDBManager = new PriorityDBManager();
        public static TicketDBManager TickerDbManager = new TicketDBManager();
        public static HistoryDBManager HistoryDBManager = new HistoryDBManager();
    }
}
