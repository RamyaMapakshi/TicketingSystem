using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.ViewModel
{
    public class History
    {
        public int ID { get; set; }

        public string ActionType { get; set; }

        public string Action { get; set; }

        public User ActionTakenBy { get; set; }

        public DateTime? ActionDateTime { get; set; }

        public int? TicketId { get; set; }

        public int? PreviousHistoryId { get; set; }

    }
}
